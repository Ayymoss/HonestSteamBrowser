using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IBlacklistRepository
{
    Task<List<EFBlacklist>> GetBlacklistAsync();
}
