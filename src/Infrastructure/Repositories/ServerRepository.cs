using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class ServerRepository(IDbContextFactory<DataContext> contextFactory, ILogger<ServerRepository> logger) : IServerRepository
{
    public async Task AddServerListAsync(IEnumerable<EFServer> newServers, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        context.Servers.AddRange(newServers);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateServerListAsync(IEnumerable<EFServer> existingServers, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        context.Servers.UpdateRange(existingServers);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var count = await context.Servers
            .AsNoTracking()
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddHours(-2))
            .SumAsync(x => x.Players, cancellationToken: cancellationToken);
        return count;
    }

    public async Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var count = await context.Servers
            .AsNoTracking()
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddHours(-2))
            .CountAsync(cancellationToken: cancellationToken);
        return count;
    }

    public async Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var serverList = await context.Servers
            .Where(x => servers.Contains(x.Hash))
            .ToListAsync(cancellationToken: cancellationToken);
        return serverList;
    }

    public async Task<Dictionary<string, double>> GetStandardDeviationsAsync(IEnumerable<string> hashes,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var snapshotGroups = await context.ServerSnapshots
            .Where(x => hashes.Contains(x.ServerHash))
            .GroupBy(x => x.ServerHash)
            .Select(g => new
            {
                Hash = g.Key,
                Snapshots = g.Select(x => x.PlayerCount).ToList()
            })
            .ToListAsync(cancellationToken);

        var deviationCounts = snapshotGroups
            .Where(x => x.Snapshots.Count > 1)
            .ToDictionary(g => g.Hash, g =>
            {
                var average = g.Snapshots.Average();
                var sumOfSquares = g.Snapshots.Sum(x => Math.Pow(x - average, 2));
                return Math.Sqrt(sumOfSquares / g.Snapshots.Count) / average;
            });

        return deviationCounts;
    }

    public async Task DeleteOldServerPlayerSnapshotsAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var serverGroupHashes = await context.ServerSnapshots
            .Where(x => x.Taken < DateTimeOffset.UtcNow.AddDays(-EFServer.OldestPlayerSnapshotInDays))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        await context.ServerSnapshots
            .Where(x => serverGroupHashes.Contains(x.Id))
            .ExecuteDeleteAsync(cancellationToken);

        logger.LogInformation("Purged {Count} player snapshots", serverGroupHashes.Count);
    }

    public async Task<List<AsnPreBlock>> GetAsnBlockListAsync(string autonomousSystemOrganization, int steamGameId,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Servers
            .Where(x => !x.Blocked)
            .Where(x => x.AutonomousSystemOrganization == autonomousSystemOrganization)
            .AsQueryable();
        if (steamGameId is not SteamGameConstants.AllGames) query = query.Where(x => x.SteamGameId == steamGameId);

        var servers = await query.Select(server => new AsnPreBlock
        {
            IpAddress = server.IpAddress,
            Port = server.Port,
            Name = server.Name,
            SteamGameId = server.SteamGameId,
            Players = server.Players,
            MaxPlayers = server.MaxPlayers,
            LastUpdated = server.LastUpdated,
            Created = server.Created,
            CountryCode = server.CountryCode,
            PlayersStandardDeviation = server.PlayersStandardDeviation,
            PlayerAverage = server.PlayerAverage ?? 0,
            PlayerUpper = server.PlayerUpperBound ?? 0,
            PlayerLower = server.PlayerLowerBound ?? 0,
        }).ToListAsync(cancellationToken: cancellationToken);

        return servers;
    }

    public async Task BlockAsnAsync(string autonomousSystemOrganization, int steamGameId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Servers.Where(x => x.AutonomousSystemOrganization == autonomousSystemOrganization);
        if (steamGameId is not SteamGameConstants.AllGames) query = query.Where(x => x.SteamGameId == steamGameId);
        var servers = await query.ToListAsync(cancellationToken: cancellationToken);
        foreach (var server in servers) server.BlockServer();
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task BlockAddressAsync(string address, int steamGameId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = context.Servers.Where(x => x.IpAddress == address);
        if (steamGameId is not SteamGameConstants.AllGames) query = query.Where(x => x.SteamGameId == steamGameId);
        var servers = await query.ToListAsync(cancellationToken: cancellationToken);
        foreach (var server in servers) server.BlockServer();
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetOlderServerHashesAsync(DateTimeOffset from, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var hashes = await context.Servers
            .Where(x => x.LastUpdated < from)
            .Select(x => x.Hash)
            .ToListAsync(cancellationToken: cancellationToken);
        return hashes;
    }

    public async Task DeleteServersByHashesAsync(List<string> serverHashes, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var servers = await context.Servers
            .Where(x => serverHashes.Contains(x.Hash))
            .ToListAsync(cancellationToken: cancellationToken);
        context.Servers.RemoveRange(servers);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetTotalPlayerCountByContinentAsync(string continent, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var mapSuccess = UtilityMethods.CountryMap.TryGetValue(continent, out var countryCodesInRegion);
        if (!mapSuccess || countryCodesInRegion is null) return 0;
        var count = await context.Servers
            .AsNoTracking()
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddHours(-2))
            .Where(server => server.CountryCode != null && countryCodesInRegion.Contains(server.CountryCode))
            .SumAsync(x => x.Players, cancellationToken: cancellationToken);
        return count;
    }

    public async Task DeletePlayerSnapshotsByServerHashesAsync(List<string> serverHashes, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var servers = await context.ServerSnapshots
            .Where(x => serverHashes.Contains(x.ServerHash))
            .ToListAsync(cancellationToken: cancellationToken);
        context.ServerSnapshots.RemoveRange(servers);
        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Purged {Count} player snapshots from server hashes", servers.Count);
    }

    public async Task<List<EFServerSnapshot>> GetServerSnapshotsAsync(string hash, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var snapshots = await context.ServerSnapshots
            .Where(x => x.ServerHash == hash)
            .ToListAsync(cancellationToken: cancellationToken);
        return snapshots;
    }
}
