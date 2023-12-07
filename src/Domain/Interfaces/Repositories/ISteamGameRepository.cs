using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface ISteamGameRepository
{
    Task<List<EFSteamGame>> GetSteamGamesAsync(CancellationToken cancellationToken = default);
    Task RemoveAsync(int steamGameId, CancellationToken cancellationToken);
    Task AddAsync(EFSteamGame steamGame, CancellationToken cancellationToken);
}
