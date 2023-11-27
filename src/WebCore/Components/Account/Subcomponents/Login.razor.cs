using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Account.Subcomponents;

public partial class Login
{
    [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;
    [Inject] private SignInManager<MyUser> SignInManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [SupplyParameterFromForm] private InputModel Input { get; set; } = new();
    
    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        if (HttpMethods.IsGet(HttpContext.Request.Method))
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }

    public async Task LoginUser()
    {
        _errorMessage = string.Empty;
   
        var result = await SignInManager.PasswordSignInAsync(Input.Username, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            NavigationManager.NavigateTo("/", true);
        }
        else
        {
            _errorMessage = "Invalid login attempt.";
        }
    }

    private sealed class InputModel
    {
        [Required, DataType(DataType.Text)] public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")] public bool RememberMe { get; set; } = true;
    }
}
