using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class CreateBlockHandler(IBlockRepository blockRepository) : INotificationHandler<CreateBlockCommand>
{
    public Task Handle(CreateBlockCommand notification, CancellationToken cancellationToken)
    {
        var efBlock = new EFBlock
        {
            Value = notification.Value,
            ApiFilter = notification.ApiFilter,
            Type = notification.Type,
            SteamGameId = notification.SteamGameId,
            UserId = notification.UserId,
            Added = DateTimeOffset.UtcNow
        };
        return blockRepository.AddAsync(efBlock, cancellationToken);
    }
}
