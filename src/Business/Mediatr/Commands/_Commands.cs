using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetInformationCommand : IRequest<CacheInfo>;

public class GetServerListCommand : Pagination, IRequest<PaginationContext<Server>>
{
    public string? UserId { get; set; }
    public bool Favourites { get; set; }
}

public class GetSteamGamesCommand : IRequest<List<SteamGame>>;

public class GetBlockCountCommand : IRequest<int>;

public class GetServerPlayersCommand : IRequest<List<PlayerInfo>?>
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
}

public class IsServerFavouriteCommand : IRequest<bool>
{
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string UserId { get; set; }
}

public class GetUserFavouriteCountCommand : IRequest<int>
{
    public string UserId { get; set; }
}

public class GetBlockListCommand : Pagination, IRequest<PaginationContext<Block>>;
public class GetSteamGameListCommand : Pagination, IRequest<PaginationContext<SteamGame>>;
