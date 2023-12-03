using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;

namespace BetterSteamBrowser.WebCore.Components.Pages.Account.Subcomponents;

public partial class RegisterComponent
{
    [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;
    [Inject] private SignInManager<ApplicationUser> SignInManager { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [SupplyParameterFromForm] private RegisterModel Input { get; set; } = new();

    private string? _errorMessage;

    protected override async Task OnInitializedAsync()
    {
        if (HttpMethods.IsGet(HttpContext.Request.Method))
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        }
    }

    public async Task RegisterUser()
    {
        _errorMessage = string.Empty;

        var user = new ApplicationUser {UserName = Input.Username};
        var result = await SignInManager.UserManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            await SignInManager.SignInAsync(user, isPersistent: false);
            NavigationManager.NavigateTo("/", true);
        }
        else
        {
            _errorMessage = result.Errors.FirstOrDefault()?.Description;
        }
    }

    private sealed class RegisterModel
    {
        [Required, DataType(DataType.Text)]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
