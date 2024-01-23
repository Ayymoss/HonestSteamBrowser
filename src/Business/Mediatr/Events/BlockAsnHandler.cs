using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class BlockAsnHandler(IServerRepository serverRepository, IBlockRepository blockRepository) : INotificationHandler<BlockAsnCommand>
{
    public async Task Handle(BlockAsnCommand notification, CancellationToken cancellationToken)
    {
        await serverRepository.BlockAsnAsync(notification.AutonomousSystemOrganization, notification.SteamGameId, cancellationToken);
        var block = new EFBlock
        {
            Value = notification.AutonomousSystemOrganization,
            ApiFilter = false,
            Type = FilterType.AutonomousSystemOrganization,
            Added = DateTimeOffset.UtcNow,
            SteamGameId = notification.SteamGameId,
            UserId = notification.UserId
        };
        await blockRepository.AddAsync(block, cancellationToken);
    }
}
