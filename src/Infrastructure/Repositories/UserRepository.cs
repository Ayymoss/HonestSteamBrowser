using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using IdentityRole = BetterSteamBrowser.Domain.Enums.IdentityRole;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task ChangeUserRoleAsync(string userId, bool isAdmin, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return;

        var adminRole = IdentityRole.Admin.ToString();
        var isInAdminRole = await userManager.IsInRoleAsync(user, adminRole);

        switch (isAdmin)
        {
            case true when !isInAdminRole:
                await userManager.AddToRoleAsync(user, adminRole);
                break;
            case false when isInAdminRole:
                await userManager.RemoveFromRoleAsync(user, adminRole);
                await userManager.UpdateSecurityStampAsync(user);
                break;
        }
    }

    public async Task ResetUserPasswordAsync(string userId, string? password, CancellationToken? cancellationToken)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return;

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var resultPassword = string.IsNullOrWhiteSpace(password) ? "BetterSteamBrowser123!" : password;
        await userManager.ResetPasswordAsync(user, token, resultPassword);
        await userManager.UpdateSecurityStampAsync(user);
    }
}
