using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class ToggleFavouriteServerHandler(IFavouriteRepository favouriteRepository) : INotificationHandler<ToggleFavouriteServerCommand>
{
    public async Task Handle(ToggleFavouriteServerCommand notification, CancellationToken cancellationToken)
    {
        await favouriteRepository.ToggleFavouriteAsync(notification.UserId, notification.IpAddress, notification.Port, cancellationToken);
    }
}
