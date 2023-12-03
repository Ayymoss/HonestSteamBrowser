using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace BetterSteamBrowser.WebCore.Components.Pages.Account;

public partial class Account
{
    [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (SignInManager.IsSignedIn(HttpContext.User))
        {
            await SignInManager.SignOutAsync();
            NavigationManager.NavigateTo("/", true);
        }

        await base.OnInitializedAsync();
    }
}
