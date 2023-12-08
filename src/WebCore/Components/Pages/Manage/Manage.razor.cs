using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;
using BetterSteamBrowser.WebCore.Components.Pages.Manage.Subcomponents;
using BetterSteamBrowser.WebCore.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityRole = BetterSteamBrowser.Domain.Enums.IdentityRole;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage;

public partial class Manage
{
    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject] private UserManager<ApplicationUser> UserManager { get; set; }

    private bool _isAdmin;
    private string? _userId;

    private UserList? _userList;
    private ServerList? _serverList;
    private BlockList? _blockList;
    private GameList? _gameList;

    protected override async Task OnInitializedAsync()
    {
        var authUser = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        _isAdmin = authUser.IsInEqualOrHigherRole(IdentityRole.Admin);
        var user = await UserManager.GetUserAsync(authUser);
        _userId = user?.Id;
        await base.OnInitializedAsync();
    }

    public async Task ReloadTables()
    {
        // I realise this isn't in the spirit of least amount of work possible, but very few people will ever use this
        await (_userList?.ReloadTable() ?? Task.CompletedTask);
        await (_serverList?.ReloadTable() ?? Task.CompletedTask);
        await (_blockList?.ReloadTable() ?? Task.CompletedTask);
        await (_gameList?.ReloadTable() ?? Task.CompletedTask);
    }
}
