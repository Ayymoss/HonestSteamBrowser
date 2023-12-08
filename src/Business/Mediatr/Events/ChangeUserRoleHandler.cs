using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class ChangeUserRoleHandler(IUserRepository userRepository) : INotificationHandler<ChangeUserRoleCommand>
{
    public Task Handle(ChangeUserRoleCommand notification, CancellationToken cancellationToken)
    {
        return userRepository.ChangeUserRoleAsync(notification.Id, notification.IsAdmin, cancellationToken);
    }
}
