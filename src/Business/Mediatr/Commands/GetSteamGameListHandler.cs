using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetSteamGameListHandler(IResourceQueryHelper<GetSteamGameListCommand, SteamGame> resourceQueryHelper)
    : IRequestHandler<GetSteamGameListCommand, PaginationContext<SteamGame>>
{
    public Task<PaginationContext<SteamGame>> Handle(GetSteamGameListCommand request, CancellationToken cancellationToken)
    {
        return resourceQueryHelper.QueryResourceAsync(request, cancellationToken);
    }
}
