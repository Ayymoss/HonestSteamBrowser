using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class GetUserFavouriteCountHandler(IFavouriteRepository favouriteRepository) : IRequestHandler<GetUserFavouriteCountCommand, int>
{
    public async Task<int> Handle(GetUserFavouriteCountCommand request, CancellationToken cancellationToken)
    {
        var count = await favouriteRepository.GetUserFavouriteCountAsync(request.UserId, cancellationToken);
        return count;
    }
}
