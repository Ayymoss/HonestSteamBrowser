using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Commands;

public class IsServerFavouriteHandler(IFavouriteRepository favouriteRepository) : IRequestHandler<IsServerFavouriteCommand, bool>
{
    public async Task<bool> Handle(IsServerFavouriteCommand request, CancellationToken cancellationToken)
    {
        var isFavourite = await favouriteRepository.IsFavouriteAsync(request.UserId, request.IpAddress, request.Port, cancellationToken);
        return isFavourite;
    }
}
