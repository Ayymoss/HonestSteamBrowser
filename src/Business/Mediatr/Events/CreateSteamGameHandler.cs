using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class CreateSteamGameHandler(ISteamGameRepository steamGameRepository) : INotificationHandler<CreateSteamGameCommand>
{
    public Task Handle(CreateSteamGameCommand notification, CancellationToken cancellationToken)
    {
        var efSteamGame = new EFSteamGame
        {
            Id = notification.AppId,
            Name = notification.Name
        };
        return steamGameRepository.AddAsync(efSteamGame, cancellationToken);
    }
    
}
