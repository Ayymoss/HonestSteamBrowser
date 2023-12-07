using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetBlockListHandler(IResourceQueryHelper<GetBlockListCommand, Block> resourceQueryHelper)
    : IRequestHandler<GetBlockListCommand, PaginationContext<Block>>
{
    public Task<PaginationContext<Block>> Handle(GetBlockListCommand request, CancellationToken cancellationToken)
    {
        return resourceQueryHelper.QueryResourceAsync(request, cancellationToken);
    }
}
