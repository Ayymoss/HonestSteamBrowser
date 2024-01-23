﻿using System.Net;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Services;
using BetterSteamBrowser.Domain.ValueObjects;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetInformationCommand : IRequest<CacheInfo>;

public class GetServerListCommand : Pagination, IRequest<PaginationContext<Server>>
{
    public string? UserId { get; set; }
    public bool Favourites { get; set; }
    public int? AppId { get; set; }
    public string? Region { get; set; }
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

public class GetServerSnapshotsCommand : IRequest<List<ServerSnapshot>>
{
    public string Hash { get; set; }
}

public class GetBlockListCommand : Pagination, IRequest<PaginationContext<Block>>;

public class GetSteamGameListCommand : Pagination, IRequest<PaginationContext<SteamGame>>;

public class GetUserListCommand : Pagination, IRequest<PaginationContext<User>>;

public class CheckAsnCountsCommand : IRequest<List<AsnPreBlock>>
{
    public string AutonomousSystemOrganization { get; set; }
    public int SteamGameId { get; set; }
}

public class GetAsnDataCommand : IRequest<AutonomousSystemData>
{
    public IPAddress IpAddress { get; set; }
}
