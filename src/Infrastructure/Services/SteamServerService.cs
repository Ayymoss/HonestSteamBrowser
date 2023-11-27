using System.Text;
using System.Text.Json;
using BetterSteamBrowser.Business.Services;
using BetterSteamBrowser.Business.Utilities;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using Serilog;

namespace BetterSteamBrowser.Infrastructure.Services;

public class SteamServerService(IHttpClientFactory httpClientFactory, IServerRepository serverRepository, SetupConfigurationContext config,
        IBlacklistRepository blacklistRepository, IGeoIpService geoIpService)
    : ISteamServerService
{
    private const int BatchSize = 10;
    private List<EFBlacklist> _blacklisted = new();

    public async Task StartSteamFetchAsync()
    {
        try
        {
            _blacklisted = await blacklistRepository.GetBlacklistAsync();
            var allStats = await RetrieveServerStatisticsAsync();
            var servers = ProcessServerStatistics(allStats).ToList();
            await serverRepository.AddAndUpdateServerListAsync(servers);
            Log.Information("Successfully modified {Count} servers", servers.Count);
        }
        catch (Exception e)
        {
            Log.Error(e, "Error executing scheduled action");
        }
    }

    private async Task<List<ServerListItem>> RetrieveServerStatisticsAsync()
    {
        var serverList = new List<ServerListItem>();
        var games = Enum.GetValues<SteamGame>()
            .Where(x => x > 0)
            .ToList();

        for (var i = 0; i < games.Count; i += BatchSize)
        {
            var batch = games.Skip(i).Take(BatchSize);
            var taskGameDictionary = new Dictionary<Task<List<ServerListItem>?>, SteamGame>();

            foreach (var game in batch)
            {
                var blacklistForApi = _blacklisted.Where(x => x.Game is SteamGame.AllGames || x.Game == game);
                var filter = BuildApiFilter(game, blacklistForApi);
                var task = GetServerListAsync(filter);
                taskGameDictionary.Add(task, game);
            }

            await Task.WhenAll(taskGameDictionary.Keys);

            foreach (var task in taskGameDictionary.Keys)
            {
                var game = taskGameDictionary[task];
                if (task.Result is null)
                {
                    Log.Information("No results for game: {Game}", game);
                    continue;
                }

                serverList.AddRange(task.Result);
            }
        }

        return serverList;
    }

    private async Task<List<ServerListItem>?> GetServerListAsync(string filterParam)
    {
        var client = httpClientFactory.CreateClient("BSBClient");
        const string endpoint = "https://api.steampowered.com/IGameServersService/GetServerList/v1/";
        var queryParams = new Dictionary<string, string>
        {
            ["key"] = config.SteamApiKey,
            ["limit"] = "20000",
            ["filter"] = filterParam
        };

        var response = await client.GetAsync($"{endpoint}?{string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}"))}");

        if (!response.IsSuccessStatusCode)
        {
            Log.Warning("Failed to request data: {Error}", response.ReasonPhrase);
            return null;
        }

        var jsonString = await response.Content.ReadAsStringAsync();
        var responseJson = JsonSerializer.Deserialize<ServerListRoot>(jsonString);
        return responseJson?.Response.Servers;
    }

    private IEnumerable<EFServer> BuildPostFilter(IEnumerable<EFServer> servers)
    {
        var blacklistsList = _blacklisted.Where(x => !x.ApiFilter).ToList();
        if (blacklistsList.Count is 0) return servers;

        var filtered = servers;

        foreach (var blacklist in blacklistsList)
        {
            switch (blacklist.Type)
            {
                case FilterType.Hostname:
                    filtered = filtered
                        .Where(x => x.Game != blacklist.Game || (x.Game == blacklist.Game && !x.Name.Contains(blacklist.Value)));
                    break;

                case FilterType.IpAddress:
                    filtered = filtered
                        .Where(x => x.Game != blacklist.Game || (x.Game == blacklist.Game && x.CountryCode != blacklist.Value));
                    break;
            }
        }

        return filtered;
    }

    private string BuildApiFilter(SteamGame game, IEnumerable<EFBlacklist> blacklists)
    {
        var blacklistsList = blacklists.Where(x => x.ApiFilter).OrderByDescending(x => x.Type).ToList();

        var filterBuilder = new StringBuilder($@"\appid\{(int)game}\empty\1\dedicated\1\name_match\*");
        filterBuilder.Append(@"\nand\1\gametype\hidden");

        if (blacklistsList.Count is 0) return filterBuilder.ToString();

        foreach (var blacklist in blacklistsList)
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

    private IEnumerable<EFServer> ProcessServerStatistics(IEnumerable<ServerListItem> allStats)
    {
        var servers = allStats
            .Where(x => x is {Name: not null, Map: not null})
            .Select(x => new EFServer
            {
                Hash = x.Address.Compute256Hash(),
                Address = x.Address,
                Name = x.Name!,
                Players = x.Players,
                MaxPlayers = x.MaxPlayers,
                Game = Enum.TryParse<SteamGame>(x.AppId.ToString(), out var game) ? game : SteamGame.Unknown,
                Country = null,
                CountryCode = null,
                Map = x.Map!,
                LastUpdated = DateTimeOffset.UtcNow
            }).ToList();
        var countryIpInformation = geoIpService.PopulateCountries(servers);
        var filtered = BuildPostFilter(countryIpInformation);

        return filtered;
    }
}
