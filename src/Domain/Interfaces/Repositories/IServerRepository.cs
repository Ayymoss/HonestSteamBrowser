using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IServerRepository
{
    Task AddAndUpdateServerListAsync(IEnumerable<EFServer> existingServers, IEnumerable<EFServer> newServers, CancellationToken cancellationToken);
    Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken);
    Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken);
    Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers);
    Task BlockAddressAsync(string address, int steamGameId, CancellationToken cancellationToken);
    Task<List<string>> GetOlderServerHashesAsync(DateTimeOffset from, CancellationToken cancellationToken);
    Task DeleteServersByHashesAsync(List<string> serverHashes, CancellationToken cancellationToken);
}
