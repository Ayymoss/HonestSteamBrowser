using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using BetterSteamBrowser.Business.Utilities;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Infrastructure.Interfaces;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;
using RestEase;

namespace BetterSteamBrowser.Infrastructure.Services;

public class SteamServerService(
    IHttpClientFactory httpClientFactory,
    IServerRepository serverRepository,
    SetupConfigurationContext config,
    IBlacklistRepository blacklistRepository,
    ISteamGameRepository steamGameRepository,
    IGeoIpService geoIpService,
    ILogger<SteamServerService> logger)
    : ISteamServerService
{
    private const int BatchSize = 10;
    private List<EFBlacklist> _blacklisted = new();
    private List<EFSteamGame> _steamGames = new();
    private ISteamApi _steamApi = null!;

    public async Task StartSteamFetchAsync()
    {
        var client = httpClientFactory.CreateClient("BSBClient");
        client.BaseAddress = new Uri("https://api.steampowered.com/");
        _steamApi = RestClient.For<ISteamApi>(client);

        try
        {
            _blacklisted = await blacklistRepository.GetBlacklistAsync();
            _steamGames = await steamGameRepository.GetSteamGamesAsync();

            var rawSteamServers = (await RetrieveServerStatisticsAsync())
                .GroupBy(serverStat => serverStat.Address.Compute256Hash())
                .ToDictionary(serverStat => serverStat.Key,
                    serverStat => serverStat.First());

            var existingServerHashes = await serverRepository.GetServerByExistingAsync(rawSteamServers.Keys);
            var existingServerDictionary = existingServerHashes.ToDictionary(server => server.Hash, server => server);

            var existingServers = existingServerDictionary
                .Select(kv => new Tuple<EFServer, ServerListItem>(kv.Value, rawSteamServers[kv.Key]))
                .ToList();

            var newServers = rawSteamServers
                .Where(kv => !existingServerDictionary.ContainsKey(kv.Key))
                .Select(kv => new Tuple<ServerListItem, string>(kv.Value, kv.Key))
                .ToList();

            var existingServerFinal = ProcessExistingServers(existingServers).ToList();
            var newServersFinal = ProcessNewServers(newServers).ToList();

            await serverRepository.AddAndUpdateServerListAsync(existingServerFinal, newServersFinal);
            logger.LogInformation("Successfully modified {Count} servers", existingServers.Count + newServersFinal.Count);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error executing scheduled action");
        }
    }

    private async Task<List<ServerListItem>> RetrieveServerStatisticsAsync()
    {
        var serverList = new ConcurrentBag<ServerListItem>();

        for (var i = 0; i < _steamGames.Count; i += BatchSize)
        {
            var batch = _steamGames.Where(x => x.Id > 0).Skip(i).Take(BatchSize);
            var tasks = batch.AsParallel().Select(ProcessGameAsync).ToList();
            var results = await Task.WhenAll(tasks);

            foreach (var result in results)
            {
                if (result is null) continue;
                foreach (var item in result.Item2) serverList.Add(item);
            }
        }

        return serverList.ToList();
    }

    private async Task<Tuple<EFSteamGame, List<ServerListItem>>?> ProcessGameAsync(EFSteamGame game)
    {
        var blacklistForApi = _blacklisted.Where(x => x.SteamGameId is SteamGameConstants.AllGames || x.SteamGameId == game.Id);
        var filter = BuildApiFilter(blacklistForApi, game.AppId);
        var serverListItems = await GetServerListAsync(filter);

        if (serverListItems is not null) return new Tuple<EFSteamGame, List<ServerListItem>>(game, serverListItems);

        logger.LogInformation("No results for game: {Game}", game.Name);
        return null;
    }

    private async Task<List<ServerListItem>?> GetServerListAsync(string filterParam)
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

            var result = await response.DeserializeHttpResponseContentAsync<ServerListRoot>();
            return result?.Response.Servers;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching server list");
            return null;
        }
    }

    private IEnumerable<EFServer> BuildBlacklist(List<EFServer> servers)
    {
        var blacklistsList = _blacklisted.Where(x => !x.ApiFilter).ToList();
        if (blacklistsList.Count is 0) return servers;

        foreach (var server in servers)
        {
            var blacklists = blacklistsList.Where(blacklist =>
                server.SteamGameId == blacklist.SteamGameId || blacklist.SteamGameId == SteamGameConstants.AllGames);
            foreach (var blacklist in blacklists)
            {
                switch (blacklist.Type)
                {
                    case FilterType.Hostname:
                        if (server.Name.Contains(blacklist.Value, StringComparison.OrdinalIgnoreCase))
                            server.Blacklisted = true;
                        break;
                    case FilterType.IpAddress:
                        if (server.CountryCode == blacklist.Value)
                            server.Blacklisted = true;
                        break;
                }
            }
        }

        return servers;
    }

    [SuppressMessage("ReSharper", "StringLiteralTypo")]
    private static string BuildApiFilter(IEnumerable<EFBlacklist> blacklists, int gameAppId)
    {
        var blacklistList = blacklists.Where(x => x.ApiFilter).OrderByDescending(x => x.Type).ToList();

        var filterBuilder = new StringBuilder($@"\appid\{gameAppId}\empty\1\dedicated\1\name_match\*");
        filterBuilder.Append(@"\nand\1\gametype\hidden");

        if (blacklistList.Count is 0) return filterBuilder.ToString();

        foreach (var blacklist in blacklistList)
        {
            switch (blacklist.Type)
            {
                case FilterType.GameType:
                    filterBuilder.Append($@"\nand\1\gametype\{blacklist.Value}");
                    break;
                case FilterType.IpAddress:
                    filterBuilder.Append($@"\nand\1\gameaddr\{blacklist.Value}");
                    break;
            }
        }

        return filterBuilder.ToString();
    }

    private IEnumerable<EFServer> ProcessExistingServers(IReadOnlyCollection<Tuple<EFServer, ServerListItem>> servers)
    {
        foreach (var server in servers.Where(server => server.Item2.Map is not null && server.Item2.Name is not null))
        {
            server.Item1.Name = server.Item2.Name!;
            server.Item1.SteamGame.AppId = server.Item2.AppId;
            server.Item1.Players = server.Item2.Players;
            server.Item1.MaxPlayers = server.Item2.MaxPlayers;
            server.Item1.Map = server.Item2.Map!;
            server.Item1.LastUpdated = DateTimeOffset.UtcNow;
        }

        var filtered = BuildBlacklist(servers.Select(x => x.Item1).ToList());
        return filtered;
    }

    private IEnumerable<EFServer> ProcessNewServers(IEnumerable<Tuple<ServerListItem, string>> steamServers)
    {
        var servers = steamServers
            .Where(x => x.Item1 is {Name: not null, Map: not null})
            .Select(x =>
            {
                var steamGameId = _steamGames.FirstOrDefault(s => s.AppId == x.Item1.AppId)?.Id ?? SteamGameConstants.Unknown;
                var address = x.Item1.Address.Split(':');

                return new EFServer
                {
                    Hash = x.Item2,
                    IpAddress = address[0],
                    Port = int.Parse(address[1]),
                    Name = x.Item1.Name!,
                    Players = x.Item1.Players,
                    MaxPlayers = x.Item1.MaxPlayers,
                    Country = null,
                    CountryCode = null,
                    Map = x.Item1.Map!,
                    LastUpdated = DateTimeOffset.UtcNow,
                    SteamGameId = steamGameId
                };
            }).ToList();

        geoIpService.PopulateCountries(servers);
        var filtered = BuildBlacklist(servers);

        return filtered;
    }
}
