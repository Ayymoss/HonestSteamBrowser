using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Services;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class UpdateInformationEvent(IServerRepository serverRepository, ServerContextCache contextCache,
        ISignalRNotification signalRNotification)
    : INotificationHandler<UpdateInformationCommand>
{
    public async Task Handle(UpdateInformationCommand notification, CancellationToken cancellationToken)
    {
        var playerCount = await serverRepository.GetTotalPlayerCountAsync(cancellationToken);
        var serverCount = await serverRepository.GetTotalServerCountAsync(cancellationToken);
        var cache = new CacheInfo
        {
            ServerCount = serverCount,
            PlayerCount = playerCount
        };

        contextCache.UpdateServerCount(cache);
        await signalRNotification.NotifyUserAsync(HubType.Main, SignalRMethod.OnInformationUpdated.ToString(), cache,
            cancellationToken);
    }
}
