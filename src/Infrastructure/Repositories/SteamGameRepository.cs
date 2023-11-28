using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class SteamGameRepository(DataContext context) : ISteamGameRepository
{
    public async Task<List<EFSteamGame>> GetSteamGamesAsync()
    {
        var result = await context.SteamGames.ToListAsync();
        return result;
    }
}
