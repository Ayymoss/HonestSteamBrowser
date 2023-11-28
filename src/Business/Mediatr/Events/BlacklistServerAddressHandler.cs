using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class BlacklistServerAddressHandler
    (IServerRepository serverRepository, IBlacklistRepository blacklistRepository) : INotificationHandler<BlacklistServerAddressCommand>
{
    public async Task Handle(BlacklistServerAddressCommand notification, CancellationToken cancellationToken)
    {
        await serverRepository.BlacklistAddressAsync(notification.IpAddress, notification.SteamGameId, cancellationToken);
        var blacklist = new EFBlacklist
        {
            Value = notification.IpAddress,
            ApiFilter = false,
            Type = FilterType.IpAddress,
            Added = DateTimeOffset.UtcNow,
            SteamGameId = notification.SteamGameId
        };
        await blacklistRepository.AddAsync(blacklist, cancellationToken);
    }
}
