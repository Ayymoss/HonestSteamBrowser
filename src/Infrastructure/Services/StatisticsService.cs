using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;

namespace BetterSteamBrowser.Infrastructure.Services;

public class StatisticsService(
    IServerRepository serverRepository,
    ISnapshotRepository snapshotRepository,
    ILogger<StatisticsService> logger)
    : IStatisticsService
{
    public async Task FetchStatisticsAsync(CancellationToken cancellationToken)
    {
        var servers = serverRepository.GetTotalServerCountAsync(cancellationToken);
        var players = serverRepository.GetTotalPlayerCountAsync(cancellationToken);

        var dateTimeOffset = DateTimeOffset.UtcNow;
        var tasks = UtilityMethods.CountryMap.Keys.Select(async continent =>
        {
            var count = await serverRepository.GetTotalPlayerCountByContinentAsync(continent, cancellationToken);
            var snapshotType = GetSnapshotType(continent);
            await snapshotRepository.SubmitSnapshotAsync(snapshotType, count, dateTimeOffset, cancellationToken);
            logger.LogDebug("Submitted statistics snapshot for {Continent} with {Count} players", continent, count);
        }).ToList();

        tasks.Add(servers);
        tasks.Add(players);

        await Task.WhenAll(tasks);

        var serversCount = servers.Result;
        var playersCount = players.Result;

        await snapshotRepository.SubmitSnapshotAsync(SnapshotType.Servers, serversCount, dateTimeOffset, cancellationToken);
        logger.LogDebug("Submitted statistics snapshot for {Continent} with {Count} players", SnapshotType.Servers.ToString(), serversCount);
        await snapshotRepository.SubmitSnapshotAsync(SnapshotType.Players, players.Result, dateTimeOffset, cancellationToken);
        logger.LogDebug("Submitted statistics snapshot for {Continent} with {Count} players", SnapshotType.Players.ToString(), playersCount);
    }

    private static SnapshotType GetSnapshotType(string type)
    {
        return type switch
        {
            "Oceania" => SnapshotType.OceaniaPlayers,
            "Asia" => SnapshotType.AsiaPlayers,
            "Africa" => SnapshotType.AfricaPlayers,
            "Americas" => SnapshotType.AmericasPlayers,
            "Europe" => SnapshotType.EuropePlayers,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
