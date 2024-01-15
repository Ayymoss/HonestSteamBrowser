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

    private record ServerData(
        List<EFServer> ExistingServerHashes,
        List<(EFServer Server, ServerListItem ServerItem)> ExistingServers,
        List<(ServerListItem ServerItem, string ServerHash)> NewServers);

    public async Task StartSteamFetchAsync(CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("BSBClient");
        client.BaseAddress = new Uri("https://api.steampowered.com/");
        _steamApi = RestClient.For<ISteamApi>(client);

        try
        {
            await PrepareSteamServiceAsync(cancellationToken);
            var servers = await GetSteamServersAsync(cancellationToken);

            logger.LogInformation("Processing {Count} servers...", servers.ExistingServers.Count + servers.NewServers.Count);
            var standardDeviations = await serverRepository
                .GetStandardDeviationsAsync(servers.ExistingServerHashes.Select(x => x.Hash), cancellationToken);
            logger.LogInformation("Fetched {Count} standard deviations", standardDeviations.Count);

            var existingServerFinal = ProcessExistingServers(servers.ExistingServers, standardDeviations).ToList();
            var newServersFinal = ProcessNewServers(servers.NewServers).ToList();

            logger.LogInformation("Saving {Count} servers...", existingServerFinal.Count + newServersFinal.Count);
            await serverRepository.UpdateServerListAsync(existingServerFinal, cancellationToken);
            await serverRepository.AddServerListAsync(newServersFinal, cancellationToken);
            logger.LogInformation("Successfully saved {Count} servers", servers.ExistingServers.Count + newServersFinal.Count);

            logger.LogInformation("Purging old player snapshots...");
            await serverRepository.DeleteOldServerPlayerSnapshotsAsync(cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error executing scheduled action");
        }
    }


    private async Task<ServerData> GetSteamServersAsync(CancellationToken cancellationToken)
    {
        var rawSteamServers = (await RetrieveServerStatisticsAsync(cancellationToken))
            .GroupBy(serverStat => serverStat.Address.Compute256Hash())
            .ToDictionary(serverStat => serverStat.Key,
                serverStat => serverStat.First());

        var existingServerHashes = await serverRepository.GetServerByExistingAsync(rawSteamServers.Keys, cancellationToken);
        var existingServerDictionary = existingServerHashes.ToDictionary(server => server.Hash, server => server);

        var existingServers = existingServerDictionary
            .Select(kv => (kv.Value, rawSteamServers[kv.Key]))
            .ToList();

        var newServers = rawSteamServers
            .Where(kv => !existingServerDictionary.ContainsKey(kv.Key))
            .Select(kv => (kv.Value, kv.Key))
            .ToList();

        return new ServerData(existingServerHashes, existingServers, newServers);
    }

    private async Task PrepareSteamServiceAsync(CancellationToken cancellationToken)
    {
        _blockList = await blockRepository.GetBlockListAsync(cancellationToken);
        _steamGames = await steamGameRepository.GetSteamGamesAsync(cancellationToken);
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

            if (result.HasValue)
                foreach (var item in result.Value.ServerItem)
                    serverList.Add(item);

            logger.LogDebug("[Finished] Fetched {GameName}", game.Name);
        });

        logger.LogInformation("[Finished] Processed {GamesCount} games", games.Count);
        return serverList.ToList();
    }

    private async Task<(EFSteamGame Game, List<ServerListItem> ServerItem)?> ProcessGameAsync(EFSteamGame game,
        CancellationToken cancellationToken)
    {
        var blocksForApi = GetBlocksForApi(game);
        var filter = BuildApiFilter(blocksForApi, game.Id);
        var serverListItems = await GetServerListAsync(filter, cancellationToken);

        logger.LogDebug("Filter {GameName}: {Filter}", game.Name, filter);

        if (serverListItems is not null) return (game, serverListItems);

        logger.LogInformation("No results for game: {GameName}", game.Name);
        return null;

        IEnumerable<EFBlock> GetBlocksForApi(EFSteamGame apiGame) => _blockList.Where(block =>
            block.SteamGameId is SteamGameConstants.AllGames || block.SteamGameId == apiGame.Id);
    }


    public async Task<List<ServerListItem>?> GetServerListAsync(string filterParam, CancellationToken cancellationToken)
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

    public IEnumerable<EFServer> BuildBlockList(List<EFServer> servers)
    {
        // Reset all servers to unblocked state as we may have removed some filters
        foreach (var server in servers) server.Blocked = false;

        Dictionary<FilterType, Func<EFServer, EFBlock, bool>> filterStrategies = new()
        {
            [FilterType.Hostname] = (server, block) => server.Name.Contains(block.Value, StringComparison.OrdinalIgnoreCase),
            [FilterType.CountryCode] = (server, block) => server.CountryCode == block.Value,
            [FilterType.IpAddress] = (server, block) => server.IpAddress.Equals(block.Value, StringComparison.CurrentCulture),
            [FilterType.Subnet] = (server, block) => server.IpAddress.IsInCidrRange(block.Value),
        };

        var blockList = _blockList.Where(x => !x.ApiFilter).ToList();
        if (blockList.Count is 0) return servers;

        foreach (var server in servers)
        {
            var blocks = blockList.Where(b => server.SteamGameId == b.SteamGameId || b.SteamGameId == SteamGameConstants.AllGames);
            foreach (var block in blocks)
                if (filterStrategies[block.Type].Invoke(server, block))
                    server.BlockServer();
        }

        return servers;
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private static string BuildApiFilter(IEnumerable<EFBlock> blocks, int gameAppId)
    {
        const string initialFilter = @"\appid\{0}\empty\1\dedicated\1\name_match\*";
        var blockList = blocks.Where(x => x.ApiFilter)
            .OrderByDescending(x => x.Type)
            .ToList();

        var filterBuilder = new StringBuilder(string.Format(initialFilter, gameAppId));
        filterBuilder.Append(@"\nand\1\gametype\hidden");

        if (blockList.Count is 0) return filterBuilder.ToString();

        foreach (var block in blockList) AppendBlockFilter(block, filterBuilder);

        return filterBuilder.ToString();

        void AppendBlockFilter(EFBlock block, StringBuilder filter)
        {
            switch (block.Type)
            {
                case FilterType.GameType:
                    filter.Append($@"\nand\1\gametype\{block.Value}");
                    break;
                case FilterType.IpAddress:
                    filter.Append($@"\nand\1\gameaddr\{block.Value}");
                    break;
            }
        }
    }

    public IEnumerable<EFServer> ProcessExistingServers(List<(EFServer Server, ServerListItem ServerItem)> servers,
        IReadOnlyDictionary<string, double> standardDeviations)
    {
        foreach (var server in servers.Where(server => server.ServerItem.Map is not null && server.ServerItem.Name is not null))
        {
            server.Server.Name = server.ServerItem.Name?.FilterEmojis() ?? "Unknown";
            server.Server.SteamGameId = server.ServerItem.AppId;
            server.Server.Players = server.ServerItem.Players;
            server.Server.MaxPlayers = server.ServerItem.MaxPlayers;
            server.Server.Map = server.ServerItem.Map!;
            server.Server.LastUpdated = DateTimeOffset.UtcNow;
            server.Server.PlayersStandardDeviation = standardDeviations.TryGetValue(server.Server.Hash, out var value) ? value : null;
            server.Server.UpdateServerStatistics();
        }

        var filtered = BuildBlockList(servers.Select(x => x.Server).ToList());
        return filtered;
    }

    public IEnumerable<EFServer> ProcessNewServers(IEnumerable<(ServerListItem ServerItem, string ServerHash)> steamServers)
    {
        var servers = steamServers
            .Where(x => x.ServerItem is {Name: not null, Map: not null})
            .Select(x =>
            {
                var address = x.ServerItem.Address.Split(':');

                return new EFServer
                {
                    Hash = x.ServerHash,
                    IpAddress = address[0],
                    Port = int.Parse(address[1]),
                    Name = x.ServerItem.Name?.FilterEmojis() ?? "Unknown",
                    Players = x.ServerItem.Players,
                    MaxPlayers = x.ServerItem.MaxPlayers,
                    Country = null,
                    CountryCode = null,
                    Map = x.ServerItem.Map!,
                    SteamGameId = x.ServerItem.AppId,
                    LastUpdated = DateTimeOffset.UtcNow,
                    Created = DateTimeOffset.UtcNow,
                };
            }).ToList();

        geoIpService.PopulateCountries(servers);
        var filtered = BuildBlockList(servers);

        return filtered;
    }
}
