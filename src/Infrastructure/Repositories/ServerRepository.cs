using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class ServerRepository(DataContext context) : IServerRepository
{
    public async Task AddAndUpdateServerListAsync(IEnumerable<EFServer> existingServers, IEnumerable<EFServer> newServers)
    {
        context.Servers.AddRange(newServers);
        context.Servers.UpdateRange(existingServers);
        await context.SaveChangesAsync();
        
        
        var test = await context.Servers.FirstOrDefaultAsync(x => x.Address == "37.61.231.78:27015");
        Console.WriteLine($"AddAndUpdateServerListAsync - 1 - {test?.Blacklisted}");
    }

    public async Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken)
    {
        var count = await context.Servers
            .Where(x => !x.Blacklisted)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddDays(-1))
            .SumAsync(x => x.Players, cancellationToken: cancellationToken);
        return count;
    }

    public async Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken)
    {
        var count = await context.Servers
            .Where(x => !x.Blacklisted)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddDays(-1))
            .CountAsync(cancellationToken: cancellationToken);
        return count;
    }

    public async Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers)
    {
        var serverList = await context.Servers
            .Where(x => servers.Contains(x.Hash))
            .Include(x => x.SteamGame)
            .ToListAsync();
        return serverList;
    }
}
