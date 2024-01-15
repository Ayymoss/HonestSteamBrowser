using BetterSteamBrowser.Business.Services;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BetterSteamBrowser.Business.Mediatr.Events;

public class UpdateInformationEvent(
    IServerRepository serverRepository,
    IServerContextCache contextCache,
    ISignalRNotification signalRNotification,
    ILogger<UpdateInformationEvent> logger)
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

        logger.LogInformation("Notifying clients of updated information");
        await signalRNotification.NotifyUsersAsync(HubType.Main, SignalRMethod.OnInformationUpdated.ToString(), cache, cancellationToken);
    }
}
