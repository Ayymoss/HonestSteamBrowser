using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class FavouriteRepository(IDbContextFactory<DataContext> contextFactory) : IFavouriteRepository
{
    public async Task<bool> IsFavouriteAsync(string userId, string ipAddress, int port, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var server = await context.Servers
            .Where(x => x.IpAddress == ipAddress && x.Port == port)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (server is null) return false;
        var favourite = await context.Favourites
            .Where(x => x.ServerId == server.Hash && x.UserId == userId)
            .AnyAsync(cancellationToken: cancellationToken);
        return favourite;
    }

    public async Task ToggleFavouriteAsync(string userId, string ipAddress, int port, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var server = await context.Servers
            .Where(x => x.IpAddress == ipAddress && x.Port == port)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (server is null) return;
        var favourite = await context.Favourites
            .Where(x => x.ServerId == server.Hash && x.UserId == userId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (favourite is null)
        {
            favourite = new EFFavourite
            {
                ServerId = server.Hash,
                UserId = userId
            };
            context.Favourites.Add(favourite);
        }
        else
        {
            context.Favourites.Remove(favourite);
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetUserFavouriteCountAsync(string userId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var count = await context.Favourites
            .Where(x => x.UserId == userId)
            .CountAsync(cancellationToken: cancellationToken);
        return count;
    }

    public async Task DeleteFavouritesByServerHashesAsync(List<string> serverHashes, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var favourites = await context.Favourites
            .Where(x => serverHashes.Contains(x.ServerId))
            .ToListAsync(cancellationToken: cancellationToken);
        context.Favourites.RemoveRange(favourites);
        await context.SaveChangesAsync(cancellationToken);
    }
}
