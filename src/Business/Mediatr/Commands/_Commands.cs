using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetInformationCommand : IRequest<CacheInfo>;
public class GetServerListCommand : Pagination, IRequest<PaginationContext<Server>>;
public class GetSteamGamesCommand : IRequest<List<SteamGame>>;
