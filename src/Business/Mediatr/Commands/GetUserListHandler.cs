using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetUserListHandler(IResourceQueryHelper<GetUserListCommand, User> resourceQueryHelper)
    : IRequestHandler<GetUserListCommand, PaginationContext<User>>
{
    public Task<PaginationContext<User>> Handle(GetUserListCommand request, CancellationToken cancellationToken)
    {
        return resourceQueryHelper.QueryResourceAsync(request, cancellationToken);
    }
}
