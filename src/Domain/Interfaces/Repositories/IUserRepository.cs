namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task ChangeUserRoleAsync(string userId, bool isAdmin, CancellationToken cancellationToken);
    Task ResetUserPasswordAsync(string userId, string? password, CancellationToken? cancellationToken);
}
