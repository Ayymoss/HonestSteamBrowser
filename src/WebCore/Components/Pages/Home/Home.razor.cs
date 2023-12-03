using BetterSteamBrowser.Infrastructure.Identity;
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
}
