using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;
using BetterSteamBrowser.WebCore.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityRole = BetterSteamBrowser.Domain.Enums.IdentityRole;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home;

public partial class Home
{
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; }

    private ServerList? _serverList;
    private WebInfoContext _webContext = new();

    private bool _isAdmin;
    private string? _userId;

    protected override async Task OnInitializedAsync()
    {
        var authUser = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        _isAdmin = authUser.IsInEqualOrHigherRole(IdentityRole.Admin);
        var user = await UserManager.GetUserAsync(authUser);
        _userId = user?.Id;
        await base.OnInitializedAsync();
    }

    private async Task OnWebContextUpdated(WebInfoContext webContext)
    {
        _webContext = webContext;
        await (_serverList?.ReloadTable() ?? Task.CompletedTask);
        StateHasChanged();
    }
}
