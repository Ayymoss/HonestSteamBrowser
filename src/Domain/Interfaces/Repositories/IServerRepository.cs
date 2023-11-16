using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IServerRepository
{
    Task AddAndUpdateServerListAsync(IReadOnlyCollection<EFServer> servers);
    Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken);
    Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken);
}
