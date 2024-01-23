using System.Diagnostics.CodeAnalysis;
using System.Net;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class BlocksPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory, IGeoIpService geoIpService)
    : IResourceQueryHelper<GetBlockListCommand, Block>
{
    public async Task<PaginationContext<Block>> QueryResourceAsync(GetBlockListCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = GetBaseQuery(context);

        int? gameSearch = request.Data is int game ? game : null;
        if (gameSearch.HasValue)
            query = ApplyGameQuery(query, gameSearch.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = ApplySearchQuery(request, query);

        if (request.Sorts.Any())
            query = ApplySortQuery(request, query);

        return await GetPagedData(request, cancellationToken, query, context);
    }

    private static IQueryable<EFBlock> GetBaseQuery(DataContext context)
    {
        return context.Blocks
            .AsNoTracking()
            .AsQueryable();
    }

    private async Task<PaginationContext<Block>> GetPagedData(GetBlockListCommand request, CancellationToken cancellationToken,
        IQueryable<EFBlock> query, DataContext context)
    {
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

        var ipAddresses = pagedData.Select(x => x.Value)
            .Select(x => IPAddress.TryParse(x, out var ip) ? ip : IPAddress.None)
            .ToList();
        var result = geoIpService.PopulateAsns(ipAddresses);
        foreach (var block in pagedData)
        {
            if (!result.TryGetValue(block.Value, out var asn)) continue;
            block.ASN = asn.AutonomousSystemNumber;
            block.ASNName = asn.AutonomousSystemOrganization;
        }

        return new PaginationContext<Block>
        {
            Data = pagedData,
            Count = count,
        };
    }

    private static IQueryable<EFBlock> ApplySortQuery(GetBlockListCommand request, IQueryable<EFBlock> query)
    {
        return request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
        {
            "Type" => current.ApplySort(sort, p => p.Type),
            "Value" => current.ApplySort(sort, p => p.Value),
            "Added" => current.ApplySort(sort, p => p.Added),
            "SteamGameName" => current.ApplySort(sort, p => p.SteamGame.Name),
            "ApiFilter" => current.ApplySort(sort, p => p.ApiFilter),
            _ => current
        });
    }

    private static IQueryable<EFBlock> ApplySearchQuery(GetBlockListCommand request, IQueryable<EFBlock> query)
    {
        return query.Where(search =>
            EF.Functions.ILike(search.Value, $"%{request.Search}%") ||
            EF.Functions.ILike(search.SteamGame.Name, $"%{request.Search}%"));
    }

    private static IQueryable<EFBlock> ApplyGameQuery(IQueryable<EFBlock> query, int gameSearch)
    {
        return query.Where(server => server.SteamGame.Id == gameSearch);
    }
}
