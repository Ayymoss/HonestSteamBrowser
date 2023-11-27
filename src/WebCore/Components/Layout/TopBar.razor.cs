using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Infrastructure.SignalR;
using Microsoft.AspNetCore.Components;

namespace BetterSteamBrowser.WebCore.Components.Layout;

public partial class TopBar : IAsyncDisposable
{
    [Inject] private BsbClientHub? HubConnection { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

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
            _ = OnInformationUpdatedReceivedAsync(cache);
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
        if (HubConnection is not null)
        {
            HubConnection.OnInformationUpdated -= OnInformationUpdatedReceived;
            await HubConnection.DisposeAsync();
        }
    }

    public void RedirectToAccount()
    {
        NavigationManager.NavigateTo("/Account", true);
    }
}
