using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class RemoveSteamGameHandler(ISteamGameRepository steamGameRepository) : INotificationHandler<RemoveSteamGameCommand>
{
    public Task Handle(RemoveSteamGameCommand notification, CancellationToken cancellationToken)
    {
        return steamGameRepository.RemoveAsync(notification.Id, cancellationToken);
    }
}
