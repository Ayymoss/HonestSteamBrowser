using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BetterSteamBrowser.Infrastructure.SignalR;

public class BsbServerHub(ISender mediator) : Hub
{
    public async Task<CacheInfo> GetInformation()
    {
        var count = await mediator.Send(new GetInformationCommand());
        return count;
    }
}
