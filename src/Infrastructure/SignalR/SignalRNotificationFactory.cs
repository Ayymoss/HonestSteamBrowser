using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace BetterSteamBrowser.Infrastructure.SignalR;

public class SignalRNotificationFactory(IHubContext<BsbServerHub> bsbHubContext) : ISignalRNotification
{
    public Task NotifyUserAsync(HubType hubType, string methodName, object message, CancellationToken cancellationToken)
    {
        return hubType switch
        {
            HubType.Main => bsbHubContext.Clients.All.SendAsync(methodName, message, cancellationToken),
            _ => throw new ArgumentOutOfRangeException($"Hub type {hubType} not supported.")
        };
    }
}

