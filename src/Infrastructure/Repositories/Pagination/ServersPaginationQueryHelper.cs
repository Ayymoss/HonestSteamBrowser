using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Entities;
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

        var query = GetBaseQuery(context);

        if (!string.IsNullOrWhiteSpace(request.Region))
            query = ApplyRegionQuery(query, request.Region);

        if (request.AppId.HasValue)
            query = ApplyAppIdQuery(query, request.AppId.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = ApplySearchQuery(query, request.Search);

        if (request.Sorts.Any())
            query = ApplySortQuery(query, request.Sorts);

        var favouriteServers = await GetFavouriteServers(request.UserId, context, cancellationToken);
        if (request.Favourites) query = ApplyFavouriteQuery(query, favouriteServers);

        return await GetPagedData(request, query, favouriteServers, cancellationToken);
    }

    private static IQueryable<EFServer> GetBaseQuery(DataContext context)
    {
        return context.Servers
            .AsNoTracking()
            .Where(x => !x.Blocked)
            .Where(server => server.LastUpdated > DateTimeOffset.UtcNow.AddHours(-2))
            .Include(x => x.SteamGame)
            .AsQueryable();
    }

    private static IQueryable<EFServer> ApplyRegionQuery(IQueryable<EFServer> query, string region)
    {
        if (UtilityMethods.CountryMap.TryGetValue(region, out var countryCodesInRegion))
            query = query.Where(server => server.CountryCode != null && countryCodesInRegion.Contains(server.CountryCode));
        return query;
    }

    private static IQueryable<EFServer> ApplyAppIdQuery(IQueryable<EFServer> query, int appId)
    {
        query = query.Where(server => server.SteamGame.Id == appId);
        return query;
    }

    private static IQueryable<EFServer> ApplySearchQuery(IQueryable<EFServer> query, string search)
    {
        var searchWords = search.Split(' ');
        var regularSearchWords = searchWords
            .Where(x => x.Length >= 3)
            .Where(word => !word.StartsWith('-'));
        var negatedSearchWords = searchWords
            .Where(x => x.Length >= 4)
            .Where(word => word.StartsWith('-'))
            .Select(word => word[1..]);

        query = regularSearchWords.Aggregate(query,
            (current, word) => current.Where(server =>
                (server.Country != null && EF.Functions.ILike(server.Country, $"%{word}%"))
                || EF.Functions.ILike(server.IpAddress, $"%{word}%")
                || EF.Functions.ILike(server.Map, $"%{word}%")
                || EF.Functions.ILike(server.Name, $"%{word}%")));

        query = negatedSearchWords.Aggregate(query,
            (current, word) => current.Where(server =>
                (server.Country == null || !EF.Functions.ILike(server.Country, $"%{word}%"))
                && !EF.Functions.ILike(server.IpAddress, $"%{word}%")
                && !EF.Functions.ILike(server.Map, $"%{word}%")
                && !EF.Functions.ILike(server.Name, $"%{word}%")));

        return query;
    }

    private static IQueryable<EFServer> ApplySortQuery(IQueryable<EFServer> query, IEnumerable<SortDescriptor> sorts)
    {
        var sortDescriptors = sorts as SortDescriptor[] ?? sorts.ToArray();

        // If we're sorting, let's remove any servers that don't have the data we're sorting by
        query = sortDescriptors.First().Property switch
        {
            "PlayersStandardDeviation" => query.Where(x => x.PlayersStandardDeviation.HasValue),
            "PlayerAverage" => query.Where(x => x.PlayerAverage.HasValue && x.PlayerAverage != 0),
            _ => query
        };

        // Apply the sort
        query = sortDescriptors.Aggregate(query, (current, sort) => sort.Property switch
        {
            "Name" => current.ApplySort(sort, p => p.Name),
            "Players" => current.ApplySort(sort, p => p.Players),
            "Country" => current.ApplySort(sort, p => p.Country ?? string.Empty),
            "PlayerAverage" => current.ApplySort(sort, p => p.PlayerAverage ?? 0),
            "Map" => current.ApplySort(sort, p => p.Map),
            "Address" => current.ApplySort(sort, p => p.IpAddress),
            "LastUpdated" => current.ApplySort(sort, p => p.LastUpdated),
            "Created" => current.ApplySort(sort, p => p.Created),
            "PlayersStandardDeviation" => current.ApplySort(sort, p => p.PlayersStandardDeviation ?? 0),
            _ => current
        });
        return query;
    }

    private static async Task<List<string>> GetFavouriteServers(string? userId, DataContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(userId)) return [];

        var favouriteServerHashes = await context.Favourites
            .Where(favourite => favourite.UserId == userId)
            .Select(favourite => favourite.ServerId)
            .ToListAsync(cancellationToken);
        return favouriteServerHashes;
    }

    private static IQueryable<EFServer> ApplyFavouriteQuery(IQueryable<EFServer> query, ICollection<string> hashes)
    {
        query = query.Where(server => hashes.Contains(server.Hash));
        return query;
    }

    private static async Task<PaginationContext<Server>> GetPagedData(GetServerListCommand request, IQueryable<EFServer> query,
        ICollection<string> favouriteServers, CancellationToken cancellationToken)
    {
        var queryServerCount = await query.CountAsync(cancellationToken: cancellationToken);
        var queryPlayerCount = 0;

        if (request.AppId.HasValue || !string.IsNullOrWhiteSpace(request.Region) || !string.IsNullOrWhiteSpace(request.Search))
            queryPlayerCount = await query.SumAsync(server => server.Players, cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(server => MapServer(favouriteServers, server))
            .ToListAsync(cancellationToken: cancellationToken);

        return new PaginationContext<Server>
        {
            Data = pagedData,
            Count = queryServerCount,
            Players = queryPlayerCount,
        };
    }

    private static Server MapServer(ICollection<string> favouriteServers, EFServer server)
    {
        return new Server
        {
            Hash = server.Hash,
            IpAddress = server.IpAddress,
            Port = server.Port,
            Name = server.Name,
            SteamGameName = server.SteamGame.Name,
            SteamGameId = server.SteamGameId,
            Map = server.Map,
            Players = server.Players,
            MaxPlayers = server.MaxPlayers,
            Country = server.Country ?? "Unknown",
            LastUpdated = server.LastUpdated,
            Created = server.Created,
            CountryCode = server.CountryCode,
            PlayersStandardDeviation = server.PlayersStandardDeviation,
            PlayerAverage = server.PlayerAverage ?? 0,
            PlayerUpper = server.PlayerUpperBound ?? 0,
            PlayerLower = server.PlayerLowerBound ?? 0,
            Favourite = favouriteServers.Contains(server.Hash),
            AutonomousSystemOrganization = server.AutonomousSystemOrganization
        };
    }
}
