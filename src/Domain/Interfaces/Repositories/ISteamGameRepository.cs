using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface ISteamGameRepository
{
    Task<List<EFSteamGame>> GetSteamGamesAsync();
}
