using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class BlacklistRepository(DataContext context) : IBlacklistRepository
{
    public async Task<List<EFBlacklist>> GetBlacklistAsync()
    {
        var blacklists = await context.Blacklists
            .Include(x => x.SteamGame)
            .ToListAsync();
        return blacklists;
    }

    public async Task AddAsync(EFBlacklist blacklist, CancellationToken cancellationToken)
    {
        var alreadyExists = await context.Blacklists
            .Where(x => x.Value == blacklist.Value && x.SteamGameId == blacklist.SteamGameId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (alreadyExists is not null) return;
        context.Blacklists.Add(blacklist);
        await context.SaveChangesAsync(cancellationToken);
    }
}
