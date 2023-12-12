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
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private BsbClientHub? HubConnection { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IHttpContextAccessor httpContextAccessor { get; set; }
    [Inject] private TooltipService TooltipService { get; set; }

    private int _players;
    private int _servers;
    private int _activeUserCount;

    protected override async Task OnInitializedAsync()
    {
        if (HubConnection is null) return;
        HubConnection.OnInformationUpdated += cacheInfo => OnInformationUpdatedReceived(cacheInfo);
        HubConnection.SiteViewerCountUpdated += count => OnUpdatePageViewersCountReceived(count);
        await HubConnection.InitializeAsync(httpContextAccessor);
        await base.OnInitializedAsync();
    }

    private Task OnUpdatePageViewersCountReceived(int count)
    {
        _activeUserCount = count;
        return InvokeAsync(StateHasChanged);
    }

    private Task OnInformationUpdatedReceived(CacheInfo cache)
    {
        _players = cache.PlayerCount;
        _servers = cache.ServerCount;
        return InvokeAsync(StateHasChanged);
    }

    public ValueTask DisposeAsync()
    {
        if (HubConnection is not null)
        {
            return HubConnection.DisposeAsync();
        }

        return ValueTask.CompletedTask;
    }

    public void RedirectToAccount()
    {
        NavigationManager.NavigateTo("/Account", true);
    }

    public async Task ManageAccountAsync()
    {
        var authUser = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        var isAuthenticated = authUser.Identity?.IsAuthenticated ?? false;
        if (!isAuthenticated) return;
        var user = await UserManager.GetUserAsync(authUser);
        if (string.IsNullOrWhiteSpace(user?.Id)) return;

        var parameters = new Dictionary<string, object>
        {
            ["UserId"] = user.Id
        };

        var options = new DialogOptions
        {
            Style = "min-height:auto;min-width:auto;width:auto;max-width:75%;max-height:97%",
            CloseDialogOnOverlayClick = true
        };

        await DialogService.OpenAsync<ManageAccountDialog>("Manage Account", parameters, options);
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options, string message) =>
        TooltipService.Open(elementReference, message, options);
}
