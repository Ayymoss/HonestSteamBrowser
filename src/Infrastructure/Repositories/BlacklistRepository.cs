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
}
