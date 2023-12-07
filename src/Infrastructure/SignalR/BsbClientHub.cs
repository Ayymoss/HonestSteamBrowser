using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;

namespace BetterSteamBrowser.Infrastructure.SignalR;

public class BsbClientHub : IAsyncDisposable
{
    public event Action<CacheInfo>? OnInformationUpdated;
    public event Action<int>? SiteViewerCountUpdated;
    private HubConnection? _hubConnection;
    private string _hubUrl = string.Empty;

    public async Task InitializeAsync(IHttpContextAccessor httpContextAccessor)
    {
        var httpContext = httpContextAccessor.HttpContext;
        var baseUri = $"{httpContext?.Request.Scheme}://{httpContext?.Request.Host}";
        _hubUrl = new Uri(new Uri(baseUri), "SignalR/MainHub").ToString();

        CreateHubConnection();
        await StartConnection();
        SubscribeToHubEvents();
    }

    private void CreateHubConnection()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .WithAutomaticReconnect()
            .Build();
    }

    private async Task StartConnection()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StartAsync();
            await InitialiseDefaults();
        }
    }

    private void SubscribeToHubEvents()
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            _hubConnection.On<CacheInfo>(SignalRMethod.OnInformationUpdated.ToString(), cache => OnInformationUpdated?.Invoke(cache));
            _hubConnection.On<int>(SignalRMethod.OnActiveUsersUpdate.ToString(), count => SiteViewerCountUpdated?.Invoke(count));
        }
    }

    private async Task InitialiseDefaults()
    {
        if (_hubConnection?.State == HubConnectionState.Connected)
        {
            var cacheInfo = await _hubConnection.InvokeAsync<CacheInfo>(SignalRMethod.GetInformation.ToString());
            var onlineCount = await _hubConnection.InvokeAsync<int>(SignalRMethod.GetActiveUsersCount.ToString());
            OnInformationUpdated?.Invoke(cacheInfo);
            SiteViewerCountUpdated?.Invoke(onlineCount);
        }
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
