using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace BetterSteamBrowser.Infrastructure.SignalR;

public class BsbServerHub(ISender mediator) : Hub
{
    private static int _activeUserCount;
    private readonly SemaphoreSlim _countLock = new(1, 1);

    public int GetActiveUsersCount() => _activeUserCount;

    private async Task UpdateAndBroadcastCount(int change)
    {
        try
        {
            await _countLock.WaitAsync();
            _activeUserCount = Math.Max(0, _activeUserCount + change);
        }
        finally
        {
            if (_countLock.CurrentCount is 0) _countLock.Release();
        }

        await Clients.All.SendAsync(SignalRMethod.OnActiveUsersUpdate.ToString(), _activeUserCount);
    }

    public override async Task OnConnectedAsync()
    {
        await UpdateAndBroadcastCount(1);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await UpdateAndBroadcastCount(-1);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<CacheInfo> GetInformation()
    {
        var count = await mediator.Send(new GetInformationCommand());
        return count;
    }
}
