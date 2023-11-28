using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IBlacklistRepository
{
    Task<List<EFBlacklist>> GetBlacklistAsync();
    Task AddAsync(EFBlacklist blacklist, CancellationToken cancellationToken);
}
