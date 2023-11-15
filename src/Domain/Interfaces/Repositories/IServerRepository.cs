using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IServerRepository
{
    Task AddAndUpdateServerListAsync(IReadOnlyCollection<EFServer> servers);
}
