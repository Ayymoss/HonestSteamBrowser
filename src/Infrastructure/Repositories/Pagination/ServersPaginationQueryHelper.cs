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

        if (!string.IsNullOrWhiteSpace(request.Region) && _countryMap.TryGetValue(request.Region, out var countryCodesInRegion))
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

    private readonly Dictionary<string, List<string>> _countryMap = new()
    {
        ["Europe"] =
        [
            "AL", "AD", "AM", "AT", "AZ", "BY", "BE", "BA", "BG", "HR", "CY", "CZ", "DK",
            "EE", "FO", "FI", "FR", "GE", "DE", "GI", "GR", "HU", "IS", "IE", "IT", "KZ",
            "XK", "LV", "LI", "LT", "LU", "MT", "MC", "ME", "NL", "MK", "NO", "PL", "PT",
            "MD", "RO", "RU", "SM", "RS", "SK", "SI", "ES", "SJ", "SE", "CH", "TR", "UA",
            "GB", "VA"
        ],
        ["Americas"] =
        [
            "AG", "BS", "BB", "BZ", "CA", "CR", "CU", "DM", "DO", "SV", "GL", "GD", "GT",
            "HT", "HN", "JM", "MX", "NI", "PA", "PR", "KN", "LC", "VC", "TT", "US", "AR",
            "BO", "BR", "CL", "CO", "EC", "FK", "GF", "GY", "PY", "PE", "SR", "UY", "VE"
        ],
        ["Asia"] =
        [
            "AF", "AM", "AZ", "BH", "BD", "BT", "BN", "KH", "CN", "CY", "GE", "IN", "ID",
            "IR", "IQ", "IL", "JP", "JO", "KZ", "KP", "KR", "KW", "KG", "LA", "LB", "MO",
            "MY", "MV", "MN", "MM", "NP", "OM", "PK", "PS", "PH", "QA", "SA", "SG", "LK",
            "SY", "TW", "TJ", "TH", "TL", "TM", "AE", "UZ", "VN", "YE"
        ],
        ["Africa"] =
        [
            "DZ", "AO", "BJ", "BW", "BF", "BI", "CV", "CM", "CF", "TD", "KM", "CG", "CD",
            "DJ", "EG", "GQ", "ER", "SZ", "ET", "GA", "GM", "GH", "GN", "GW", "CI", "KE",
            "LS", "LR", "LY", "MG", "MW", "ML", "MR", "MU", "YT", "MA", "MZ", "NA", "NE",
            "NG", "RE", "RW", "SH", "ST", "SN", "SC", "SL", "SO", "ZA", "SS", "SD", "TZ",
            "TG", "TN", "UG", "EH", "ZM", "ZW"
        ],
        ["Oceania"] =
        [
            "AS", "AU", "CK", "FJ", "PF", "GU", "KI", "MH", "FM", "NR", "NC", "NZ", "NU",
            "NF", "MP", "PW", "PG", "PN", "WS", "SB", "TK", "TO", "TV", "UM", "VU", "WF"
        ]
    };
}
