using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IServerRepository
{
    Task AddAndUpdateServerListAsync(IEnumerable<EFServer> existingServers, IEnumerable<EFServer> newServers);
    Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken);
    Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken);
    Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers);
}
