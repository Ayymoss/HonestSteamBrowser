using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class ServerRepository(IDbContextFactory<DataContext> contextFactory) : IServerRepository
{
    public async Task AddAndUpdateServerListAsync(IEnumerable<EFServer> existingServers, IEnumerable<EFServer> newServers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        context.Servers.AddRange(newServers);
        context.Servers.UpdateRange(existingServers);
        await context.SaveChangesAsync();
    }

    public async Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var count = await context.Servers
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddDays(-1))
            .SumAsync(x => x.Players, cancellationToken: cancellationToken);
        return count;
    }

    public async Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var count = await context.Servers
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddDays(-1))
            .CountAsync(cancellationToken: cancellationToken);
        return count;
    }

    public async Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var serverList = await context.Servers
            .Where(x => servers.Contains(x.Hash))
            .Include(x => x.SteamGame)
            .ToListAsync();
        return serverList;
    }

    public async Task BlockAddressAsync(string address, int steamGameId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Servers.Where(x => x.IpAddress == address);
        if (steamGameId is not SteamGameConstants.AllGames) query = query.Where(x => x.SteamGameId == steamGameId);
        var servers = await query.ToListAsync(cancellationToken: cancellationToken);
        foreach (var server in servers) server.Blocked = true;
        await context.SaveChangesAsync(cancellationToken);
    }
}
