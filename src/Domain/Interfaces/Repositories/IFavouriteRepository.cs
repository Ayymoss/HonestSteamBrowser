namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IFavouriteRepository
{
    Task<bool> IsFavouriteAsync(string userId, string ipAddress, int port, CancellationToken cancellationToken);
    Task ToggleFavouriteAsync(string userId, string ipAddress, int port, CancellationToken cancellationToken);
    Task<int> GetUserFavouriteCountAsync(string userId, CancellationToken cancellationToken);
}
