using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace BetterSteamBrowser.Infrastructure.Identity;

public class ClaimsTransformer(UserManager<ApplicationUser> userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity is not ClaimsIdentity identity || identity.HasClaim(c => c.Type == ClaimTypes.Role)) return principal;
        var user = await userManager.GetUserAsync(principal);
        if (user is null) return principal;

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        return principal;
    }
}
