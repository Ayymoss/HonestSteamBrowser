using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class BlacklistRepository(IDbContextFactory<DataContext> contextFactory) : IBlacklistRepository
{
    public async Task<List<EFBlacklist>> GetBlacklistAsync()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var blacklists = await context.Blacklists
            .Include(x => x.SteamGame)
            .ToListAsync();
        return blacklists;
    }

    public async Task AddAsync(EFBlacklist blacklist, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var alreadyExists = await context.Blacklists
            .Where(x => x.Value == blacklist.Value && x.SteamGameId == blacklist.SteamGameId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (alreadyExists is not null) return;
        context.Blacklists.Add(blacklist);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var count = await context.Blacklists.CountAsync(cancellationToken: cancellationToken);
        return count;
    }
}
