using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class BlockServerAddressHandler
    (IServerRepository serverRepository, IBlockRepository blockRepository) : INotificationHandler<BlockServerAddressCommand>
{
    public async Task Handle(BlockServerAddressCommand notification, CancellationToken cancellationToken)
    {
        await serverRepository.BlockAddressAsync(notification.IpAddress, notification.SteamGameId, cancellationToken);
        var block = new EFBlock
        {
            Value = notification.IpAddress,
            ApiFilter = false,
            Type = FilterType.IpAddress,
            Added = DateTimeOffset.UtcNow,
            SteamGameId = notification.SteamGameId,
            UserId = notification.UserId
        };
        await blockRepository.AddAsync(block, cancellationToken);
    }
}
