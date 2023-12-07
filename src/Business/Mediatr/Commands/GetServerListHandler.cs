using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetServerListHandler(IResourceQueryHelper<GetServerListCommand, Server> resourceQueryHelper)
    : IRequestHandler<GetServerListCommand, PaginationContext<Server>>
{
    public async Task<PaginationContext<Server>> Handle(GetServerListCommand request, CancellationToken cancellationToken)
    {
        var result = await resourceQueryHelper.QueryResourceAsync(request, cancellationToken);
        return result;
    }
}
