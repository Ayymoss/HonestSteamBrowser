using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class ResetUserPasswordHandler(IUserRepository userRepository) : INotificationHandler<ResetUserPasswordCommand>
{
    public Task Handle(ResetUserPasswordCommand notification, CancellationToken cancellationToken)
    {
        return userRepository.ResetUserPasswordAsync(notification.Id, notification.Password, cancellationToken);
    }
}
