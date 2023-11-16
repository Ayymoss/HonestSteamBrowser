using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;

namespace BetterSteamBrowser.Infrastructure.SignalR;

public class BsbClientHub : IAsyncDisposable
{
    public event Action<CacheInfo>? OnInformationUpdated;
    private HubConnection? _hubConnection;

    public async Task InitializeAsync()
    {
        CreateHubConnection();
        await StartConnection();
        SubscribeToHubEvents();
        await InitialiseDefaults();
    }

    private void CreateHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
#if DEBUG
            .WithUrl("http://localhost:8123/SignalR/MainHub")
#else
            .WithUrl("https://bettersteambrowsers.app/SignalR/MainHub")
#endif
            .WithAutomaticReconnect()
            .Build();
    }

    private async Task StartConnection()
    {
        if (_hubConnection is not null) await _hubConnection.StartAsync();
    }

    private void SubscribeToHubEvents()
    {
        _hubConnection?.On<CacheInfo>(SignalRMethods.OnInformationUpdated.ToString(), cache => OnInformationUpdated?.Invoke(cache));
    }

    private async Task InitialiseDefaults()
    {
        if (_hubConnection is null) return;
        var onlineCount = await _hubConnection.InvokeAsync<CacheInfo>(SignalRMethods.GetInformation.ToString());
        OnInformationUpdated?.Invoke(onlineCount);
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is {State: HubConnectionState.Connected})
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
        }
    }
}
