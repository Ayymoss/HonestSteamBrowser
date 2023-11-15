using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class ServerRepository(DataContext context) : IServerRepository
{
    public async Task AddAndUpdateServerListAsync(IReadOnlyCollection<EFServer> servers)
    {
        var distinctServerHashes = servers.Select(s => s.Hash).Distinct().ToList();
        var hashList = await context.Servers
            .Where(x => distinctServerHashes.Contains(x.Hash))
            .Select(s => s.Hash)
            .ToListAsync();

        var existingHashes = new HashSet<string>(hashList);
        var newServers = servers.Where(s => !existingHashes.Contains(s.Hash)).ToList();
        var updatedServers = servers.Where(s => existingHashes.Contains(s.Hash)).ToList();

        context.Servers.AddRange(newServers);
        context.Servers.UpdateRange(updatedServers);

        await context.SaveChangesAsync();
    }
}
