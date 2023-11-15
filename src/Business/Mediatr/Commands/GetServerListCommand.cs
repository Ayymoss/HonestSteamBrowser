using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetServerListCommand : Pagination, IRequest<PaginationContext<Server>>
{
}
