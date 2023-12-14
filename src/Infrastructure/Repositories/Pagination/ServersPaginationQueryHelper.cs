using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class ServersPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory)
    : IResourceQueryHelper<GetServerListCommand, Server>
{
    public async Task<PaginationContext<Server>> QueryResourceAsync(GetServerListCommand request,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Servers
            .AsNoTracking()
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddHours(-2))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Region) &&
            UtilityMethods.CountryMap.TryGetValue(request.Region, out var countryCodesInRegion))
            query = query.Where(server => server.CountryCode != null && countryCodesInRegion.Contains(server.CountryCode));

        if (request.AppId.HasValue) query = query.Where(server => server.SteamGame.Id == request.AppId);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = request.Search.Split(' ')
                .Aggregate(query, (current, searchWord) => current.Where(search =>
                    (search.Country != null && EF.Functions.ILike(search.Country, $"%{searchWord}%"))
                    || EF.Functions.ILike(search.IpAddress, $"%{searchWord}%")
                    || EF.Functions.ILike(search.Map, $"%{searchWord}%")
                    || EF.Functions.ILike(search.Name, $"%{searchWord}%")));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "Name" => current.ApplySort(sort, p => p.Name),
                "Players" => current.ApplySort(sort, p => p.Players),
                "Country" => current.ApplySort(sort, p => p.Country ?? string.Empty),
                "Map" => current.ApplySort(sort, p => p.Map),
                "Address" => current.ApplySort(sort, p => p.IpAddress),
                "LastUpdated" => current.ApplySort(sort, p => p.LastUpdated),
                "Created" => current.ApplySort(sort, p => p.Created),
                _ => current
            });

        var favouriteServers = new Dictionary<string, bool>();
        var favouriteServerHashes = new List<string>();

        if (!string.IsNullOrWhiteSpace(request.UserId))
        {
            favouriteServerHashes = await context.Favourites
                .Where(favourite => favourite.UserId == request.UserId)
                .Select(favourite => favourite.ServerId)
                .ToListAsync(cancellationToken);

            foreach (var hash in favouriteServerHashes)
            {
                favouriteServers[hash] = true;
            }
        }

        if (request.Favourites)
        {
            query = query.Where(server => favouriteServerHashes.Contains(server.Hash));
        }

        var queryServerCount = await query.CountAsync(cancellationToken: cancellationToken);
        var queryPlayerCount = 0;

        if (request.AppId.HasValue || !string.IsNullOrWhiteSpace(request.Region) || !string.IsNullOrWhiteSpace(request.Search))
            queryPlayerCount = await query.SumAsync(server => server.Players, cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(server => new Server
            {
                IpAddress = server.IpAddress,
                Port = server.Port,
                Name = server.Name,
                SteamGameAppId = server.SteamGame.Id,
                SteamGameName = server.SteamGame.Name,
                SteamGameId = server.SteamGameId,
                Map = server.Map,
                Players = server.Players,
                MaxPlayers = server.MaxPlayers,
                Country = server.Country ?? "Unknown",
                LastUpdated = server.LastUpdated,
                Created = server.Created,
                Favourite = favouriteServers.ContainsKey(server.Hash) && favouriteServers[server.Hash]
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<Server>
        {
            Data = pagedData,
            Count = queryServerCount,
            Players = queryPlayerCount
        };
    }
}
