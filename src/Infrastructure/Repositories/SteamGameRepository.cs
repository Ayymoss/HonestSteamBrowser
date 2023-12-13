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

    public async Task RemoveAsync(int steamGameId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var block = await context.SteamGames.FindAsync(new object?[] {steamGameId}, cancellationToken: cancellationToken);
        if (block is null) return;
        context.SteamGames.Remove(block);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(EFSteamGame steamGame, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var alreadyExists = await context.SteamGames
            .Where(x => x.Id == steamGame.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (alreadyExists is not null) return;
        context.SteamGames.Add(steamGame);
        await context.SaveChangesAsync(cancellationToken);
    }
}
