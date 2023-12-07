using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class RemoveBlockHandler(IBlockRepository blockRepository) : INotificationHandler<RemoveBlockCommand>
{
    public Task Handle(RemoveBlockCommand notification, CancellationToken cancellationToken)
    {
        return blockRepository.RemoveAsync(notification.Id, cancellationToken);
    }
}
