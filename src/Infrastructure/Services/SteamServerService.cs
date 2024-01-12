using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BetterSteamBrowser.Business.Utilities;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Infrastructure.Interfaces;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using RestEase;

namespace BetterSteamBrowser.Infrastructure.Services;

// TODO: Delay showing new servers for a day or so. This will allow us to filter out servers that are only up for a short time
// TODO: Trust Score. If a server is up for a long time, it's more likely to be a good server
// TODO: Standard dev for each server's player history. Consistency shows spoofed servers
// TODO: We have player history, let's use it in the algo for determining spoofed servers

public class SteamServerService(
    IHttpClientFactory httpClientFactory,
    IServerRepository serverRepository,
    SetupConfigurationContext config,
    IBlockRepository blockRepository,
    ISteamGameRepository steamGameRepository,
    IGeoIpService geoIpService,
    ILogger<SteamServerService> logger)
    : ISteamServerService
{
    private List<EFBlock> _blockList = [];
    private List<EFSteamGame> _steamGames = [];
    private ISteamApi _steamApi = null!;

    public async Task StartSteamFetchAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BSBClient");
        client.BaseAddress = new Uri("https://api.steampowered.com/");
        _steamApi = RestClient.For<ISteamApi>(client);

        try
        {
            _blockList = await blockRepository.GetBlockListAsync(cancellationToken);
            _steamGames = await steamGameRepository.GetSteamGamesAsync(cancellationToken);

            var rawSteamServers = (await RetrieveServerStatisticsAsync(cancellationToken))
                .GroupBy(serverStat => serverStat.Address.Compute256Hash())
                .ToDictionary(serverStat => serverStat.Key,
                    serverStat => serverStat.First());

            logger.LogInformation("Processing {Count} servers...", rawSteamServers.Count);
            var existingServerHashes = await serverRepository.GetServerByExistingAsync(rawSteamServers.Keys);
            var existingServerDictionary = existingServerHashes.ToDictionary(server => server.Hash, server => server);

            var existingServers = existingServerDictionary
                .Select(kv => new Tuple<EFServer, ServerListItem>(kv.Value, rawSteamServers[kv.Key]))
                .ToList();

            var newServers = rawSteamServers
                .Where(kv => !existingServerDictionary.ContainsKey(kv.Key))
                .Select(kv => new Tuple<ServerListItem, string>(kv.Value, kv.Key))
                .ToList();

            var standardDeviations = await serverRepository
                .GetStandardDeviationsAsync(existingServerHashes.Select(x => x.Hash), cancellationToken);
            logger.LogInformation("Fetched {Count} standard deviations", standardDeviations.Count);

            var existingServerFinal = ProcessExistingServers(existingServers, standardDeviations).ToList();
            var newServersFinal = ProcessNewServers(newServers).ToList();

            logger.LogInformation("Saving {Count} servers...", existingServerFinal.Count + newServersFinal.Count);
            await serverRepository.UpdateServerListAsync(existingServerFinal, cancellationToken);
            await serverRepository.AddServerListAsync(newServersFinal, cancellationToken);

            logger.LogInformation("Purging old player snapshots...");
            await serverRepository.DeleteOldServerPlayerSnapshotsAsync(cancellationToken);
            logger.LogInformation("Successfully saved {Count} servers", existingServers.Count + newServersFinal.Count);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error executing scheduled action");
        }
    }

    private async Task<List<ServerListItem>> RetrieveServerStatisticsAsync(CancellationToken cancellationToken)
    {
        var serverList = new ConcurrentBag<ServerListItem>();
        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = Environment.ProcessorCount,
            CancellationToken = cancellationToken
        };

        var games = _steamGames.Where(x => x.Id > 0).ToList();

        logger.LogInformation("[Starting] Processing {GamesCount} games", games.Count);

        // Token comes from the options.
        await Parallel.ForEachAsync(games, parallelOptions, async (game, token) =>
        {
            logger.LogDebug("[Starting] Fetching {GameName}", game.Name);
            var result = await ProcessGameAsync(game, token);

            if (result is not null)
            {
                foreach (var item in result.Item2)
                {
                    serverList.Add(item);
                }
            }

            logger.LogDebug("[Finished] Fetched {GameName}", game.Name);
        });

        logger.LogInformation("[Finished] Processed {GamesCount} games", games.Count);
        return serverList.ToList();
    }

    private async Task<Tuple<EFSteamGame, List<ServerListItem>>?> ProcessGameAsync(EFSteamGame game, CancellationToken cancellationToken)
    {
        var blocksForApi = _blockList.Where(x => x.SteamGameId is SteamGameConstants.AllGames || x.SteamGameId == game.Id);
        var filter = BuildApiFilter(blocksForApi, game.Id);
        var serverListItems = await GetServerListAsync(filter, cancellationToken);
        logger.LogDebug("Filter {GameName}: {Filter}", game.Name, filter);

        if (serverListItems is not null) return new Tuple<EFSteamGame, List<ServerListItem>>(game, serverListItems);

        logger.LogInformation("No results for game: {Game}", game.Name);
        return null;
    }

    private async Task<List<ServerListItem>?> GetServerListAsync(string filterParam, CancellationToken cancellationToken)
    {
        const int queryLimit = 20_000;
        var queryParams = new Dictionary<string, string>
        {
            ["key"] = config.SteamApiKey,
            ["limit"] = queryLimit.ToString(),
            ["filter"] = filterParam
        };

        try
        {
            var response = await _steamApi.GetServerListAsync(queryParams);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning("Failed to request data: {Error}", response.ReasonPhrase);
                return null;
            }

            var result = await response.DeserializeHttpResponseContentAsync<ServerListRoot>(cancellationToken);
            return result?.Response.Servers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching server list");
            return null;
        }
    }

    private IEnumerable<EFServer> BuildBlockList(List<EFServer> servers)
    {
        // Reset all servers to unblocked state as we may have removed some filters
        foreach (var server in servers) server.Blocked = false;

        var blockList = _blockList.Where(x => !x.ApiFilter).ToList();
        if (blockList.Count is 0) return servers;

        foreach (var server in servers)
        {
            var blocks = blockList.Where(b =>
                server.SteamGameId == b.SteamGameId || b.SteamGameId == SteamGameConstants.AllGames);
            foreach (var block in blocks)
            {
                switch (block.Type)
                {
                    case FilterType.Hostname:
                        if (server.Name.Contains(block.Value, StringComparison.OrdinalIgnoreCase))
                            server.BlockServer();
                        break;
                    case FilterType.CountryCode:
                        if (server.CountryCode == block.Value)
                            server.BlockServer();
                        break;
                    case FilterType.IpAddress:
                        if (server.IpAddress.Equals(block.Value, StringComparison.CurrentCulture))
                            server.BlockServer();
                        break;
                    case FilterType.Subnet:
                        if (server.IpAddress.IsInCidrRange(block.Value))
                            server.BlockServer();
                        break;
                }
            }
        }

        return servers;
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private static string BuildApiFilter(IEnumerable<EFBlock> blocks, int gameAppId)
    {
        var blockList = blocks.Where(x => x.ApiFilter)
            .OrderByDescending(x => x.Type)
            .ToList();

        var filterBuilder = new StringBuilder($@"\appid\{gameAppId}\empty\1\dedicated\1\name_match\*");
        filterBuilder.Append(@"\nand\1\gametype\hidden");
        if (blockList.Count is 0) return filterBuilder.ToString();

        foreach (var block in blockList)
        {
            switch (block.Type)
            {
                case FilterType.GameType:
                    filterBuilder.Append($@"\nand\1\gametype\{block.Value}");
                    break;
                case FilterType.IpAddress:
                    filterBuilder.Append($@"\nand\1\gameaddr\{block.Value}");
                    break;
            }
        }

        return filterBuilder.ToString();
    }

    private IEnumerable<EFServer> ProcessExistingServers(IReadOnlyCollection<Tuple<EFServer, ServerListItem>> servers,
        IReadOnlyDictionary<string, double> standardDeviations)
    {
        foreach (var server in servers.Where(server => server.Item2.Map is not null && server.Item2.Name is not null))
        {
            server.Item1.Name = server.Item2.Name?.FilterEmojis() ?? "Unknown";
            server.Item1.SteamGameId = server.Item2.AppId;
            server.Item1.Players = server.Item2.Players;
            server.Item1.MaxPlayers = server.Item2.MaxPlayers;
            server.Item1.Map = server.Item2.Map!;
            server.Item1.LastUpdated = DateTimeOffset.UtcNow;
            server.Item1.PlayersStandardDeviation = standardDeviations.TryGetValue(server.Item1.Hash, out var value) ? value : null;
            server.Item1.UpdateServerStatistics();
        }

        var filtered = BuildBlockList(servers.Select(x => x.Item1).ToList());
        return filtered;
    }

    private IEnumerable<EFServer> ProcessNewServers(IEnumerable<Tuple<ServerListItem, string>> steamServers)
    {
        var servers = steamServers
            .Where(x => x.Item1 is {Name: not null, Map: not null})
            .Select(x =>
            {
                var address = x.Item1.Address.Split(':');

                return new EFServer
                {
                    Hash = x.Item2,
                    IpAddress = address[0],
                    Port = int.Parse(address[1]),
                    Name = x.Item1.Name?.FilterEmojis() ?? "Unknown",
                    Players = x.Item1.Players,
                    MaxPlayers = x.Item1.MaxPlayers,
                    Country = null,
                    CountryCode = null,
                    Map = x.Item1.Map!,
                    SteamGameId = x.Item1.AppId,
                    LastUpdated = DateTimeOffset.UtcNow,
                    Created = DateTimeOffset.UtcNow,
                };
            }).ToList();

        geoIpService.PopulateCountries(servers);
        var filtered = BuildBlockList(servers);

        return filtered;
    }
}
