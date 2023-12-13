using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class SteamGamesPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory)
    : IResourceQueryHelper<GetSteamGameListCommand, SteamGame>
{
    public async Task<PaginationContext<SteamGame>> QueryResourceAsync(GetSteamGameListCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.SteamGames
            .AsNoTracking()
            .Where(x => x.Id > 0)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(search =>
                EF.Functions.ILike(search.Name, $"%{request.Search}%") ||
                EF.Functions.ILike(search.Id.ToString(), $"%{request.Search}%"));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "Name" => current.ApplySort(sort, p => p.Name),
                "AppId" => current.ApplySort(sort, p => p.Id),
                _ => current
            });

        var count = await query.CountAsync(cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(x => new SteamGame
            {
                AppId = x.Id,
                Name = x.Name,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<SteamGame>
        {
            Data = pagedData,
            Count = count,
        };
    }
}
