using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Infrastructure.SignalR;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace BetterSteamBrowser.WebCore.Components.Layout;

public partial class MainLayout : IAsyncDisposable
{
    [Inject] private BsbClientHub? HubConnection { get; set; }

    private RadzenBody? _body;

    private int _players;
    private int _servers;

    protected override async Task OnInitializedAsync()
    {
        if (HubConnection is null) return;
        HubConnection.OnInformationUpdated += OnInformationUpdatedReceived;
        await HubConnection.InitializeAsync();
        await base.OnInitializedAsync();
    }

    private void OnInformationUpdatedReceived(CacheInfo cache)
    {
        try
        {
            _ = OnInformationUpdatedReceivedAsync(cache).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
    
    private async Task OnInformationUpdatedReceivedAsync(CacheInfo cache)
    {
        _players = cache.PlayerCount;
        _servers = cache.ServerCount;
        await InvokeAsync(StateHasChanged);
    }

    public async ValueTask DisposeAsync()
    {
        _body?.Dispose();
        if (HubConnection is not null)
        {
            HubConnection.OnInformationUpdated -= OnInformationUpdatedReceived;
            await HubConnection.DisposeAsync();
        }
    }
}
