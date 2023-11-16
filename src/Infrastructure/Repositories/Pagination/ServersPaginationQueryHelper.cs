using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class ServersPaginationQueryHelper(DataContext context) : IResourceQueryHelper<GetServerListCommand, Server>
{
    public async Task<PaginationContext<Server>> QueryResourceAsync(GetServerListCommand request,
        CancellationToken cancellationToken)
    {
        var query = context.Servers
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddDays(-1))
            .AsQueryable();

        SteamGame? gameSearch = request.Data is SteamGame game ? game : null;
        if (gameSearch is not null && gameSearch is not SteamGame.AllGames) query = query.Where(server => server.Game == gameSearch);

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            query = query.Where(search =>
                EF.Functions.ILike(search.Address, $"%{request.SearchString}%") ||
                EF.Functions.ILike(search.Name, $"%{request.SearchString}%"));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "Name" => current.ApplySort(sort, p => p.Name),
                "Players" => current.ApplySort(sort, p => p.Players),
                "LastUpdated" => current.ApplySort(sort, p => p.LastUpdated),
                _ => current
            });

        var count = await query.CountAsync(cancellationToken: cancellationToken);
        var playerCount = 0;
        if (gameSearch is not null)
            playerCount = await query.SumAsync(server => server.Players, cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(server => new Server
            {
                Address = server.Address,
                Name = server.Name,
                Game = server.Game,
                Map = server.Map,
                Players = server.Players,
                MaxPlayers = server.MaxPlayers,
                Region = server.Region,
                LastUpdated = server.LastUpdated
            }).ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<Server>
        {
            Data = pagedData,
            Count = count,
            Players = playerCount
        };
    }
}
