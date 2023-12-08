using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.Infrastructure.SignalR;
using BetterSteamBrowser.WebCore.Components.Dialogs;
using BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;
using BetterSteamBrowser.WebCore.Utilities;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Radzen;
using IdentityRole = BetterSteamBrowser.Domain.Enums.IdentityRole;

namespace BetterSteamBrowser.WebCore.Components.Layout;

public partial class TopBar : IAsyncDisposable
{
    // TODO: Move Password Reset to a more appropriate location ResetPasswordAsync
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    // END

    [Inject] private BsbClientHub? HubConnection { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IHttpContextAccessor httpContextAccessor { get; set; }

    private int _players;
    private int _servers;
    private int _activeUserCount;

    protected override async Task OnInitializedAsync()
    {
        if (HubConnection is null) return;
        HubConnection.OnInformationUpdated += OnInformationUpdatedReceived;
        HubConnection.SiteViewerCountUpdated += UpdatePageViewersCountReceived;
        await HubConnection.InitializeAsync(httpContextAccessor);
        await base.OnInitializedAsync();
    }

    private void UpdatePageViewersCountReceived(int count)
    {
        try
        {
            _ = UpdatePageViewersCountAsync(count);
        }
        catch
        {
            // ignored
        }
    }

    private async Task UpdatePageViewersCountAsync(int count)
    {
        _activeUserCount = count;
        await InvokeAsync(StateHasChanged);
    }

    private void OnInformationUpdatedReceived(CacheInfo cache)
    {
        try
        {
            _ = OnInformationUpdatedReceivedAsync(cache);
        }
        catch
        {
            // ignored
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

    public async Task ResetPasswordAsync()
    {
        var authUser = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var isAuthenticated = authUser.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated) return;
        var user = await UserManager.GetUserAsync(authUser);
        if (string.IsNullOrWhiteSpace(user?.Id)) return;

        var parameters = new Dictionary<string, object>
        {
            {"UserId", user.Id}
        };

        var options = new DialogOptions
        {
            Style = "min-height:auto;min-width:auto;width:auto;max-width:75%;max-height:97%",
            CloseDialogOnOverlayClick = true
        };

        await DialogService.OpenAsync<PasswordResetDialog>("Reset Password?", parameters, options);
    }
}
