using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class SteamGameRepository(IDbContextFactory<DataContext> contextFactory) : ISteamGameRepository
{
    public async Task<List<EFSteamGame>> GetSteamGamesAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var result = await context.SteamGames.ToListAsync(cancellationToken: cancellationToken);
        return result;
    }
}
