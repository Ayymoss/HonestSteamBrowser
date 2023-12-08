using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class BlocksPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory) : IResourceQueryHelper<GetBlockListCommand, Block>
{
    public async Task<PaginationContext<Block>> QueryResourceAsync(GetBlockListCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Blocks
            .AsNoTracking()
            .AsQueryable();

        int? gameSearch = request.Data is int game ? game : null;
        if (gameSearch is not null) query = query.Where(server => server.SteamGame.AppId == gameSearch);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            query = query.Where(search =>
                EF.Functions.ILike(search.Value, $"%{request.SearchString}%") ||
                EF.Functions.ILike(search.SteamGame.Name, $"%{request.SearchString}%"));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "Type" => current.ApplySort(sort, p => p.Type),
                "Value" => current.ApplySort(sort, p => p.Value),
                "Added" => current.ApplySort(sort, p => p.Added),
                "SteamGameName" => current.ApplySort(sort, p => p.SteamGame.Name),
                "ApiFilter" => current.ApplySort(sort, p => p.ApiFilter),
                _ => current
            });

        var count = await query.CountAsync(cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Join(context.Users,
                block => block.UserId,
                user => user.Id,
                (block, user) => new {Block = block, User = user})
            .Select(bu => new Block
            {
                Id = bu.Block.Id,
                Value = bu.Block.Value,
                Type = bu.Block.Type,
                Added = bu.Block.Added,
                SteamGameName = bu.Block.SteamGame.Name,
                ApiFilter = bu.Block.ApiFilter,
                AddedBy = bu.User.UserName ?? "< UNSET >",
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<Block>
        {
            Data = pagedData,
            Count = count,
        };
    }
}
