using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.ValueObjects;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IServerRepository
{
    Task AddServerListAsync(IEnumerable<EFServer> newServers, CancellationToken cancellationToken);
    Task UpdateServerListAsync(IEnumerable<EFServer> existingServers, CancellationToken cancellationToken);
    Task<int> GetTotalPlayerCountAsync(CancellationToken cancellationToken);
    Task<int> GetTotalServerCountAsync(CancellationToken cancellationToken);
    Task<List<EFServer>> GetServerByExistingAsync(IEnumerable<string> servers, CancellationToken cancellationToken);
    Task BlockAddressAsync(string address, int steamGameId, CancellationToken cancellationToken);
    Task<List<string>> GetOlderServerHashesAsync(DateTimeOffset from, CancellationToken cancellationToken);
    Task DeleteServersByHashesAsync(List<string> serverHashes, CancellationToken cancellationToken);
    Task<int> GetTotalPlayerCountByContinentAsync(string continent, CancellationToken cancellationToken);
    Task DeletePlayerSnapshotsByServerHashesAsync(List<string> serverHashes, CancellationToken cancellationToken);
    Task<List<EFServerSnapshot>> GetServerSnapshotsAsync(string hash, CancellationToken cancellationToken);
    Task<Dictionary<string, double>> GetStandardDeviationsAsync(IEnumerable<string> hashes, CancellationToken cancellationToken);
    Task DeleteOldServerPlayerSnapshotsAsync(CancellationToken cancellationToken);
    Task<List<AsnPreBlock>> GetAsnBlockListAsync(string autonomousSystemOrganization, int steamGameId, CancellationToken cancellationToken);
    Task BlockAsnAsync(string autonomousSystemOrganization, int steamGameId, CancellationToken cancellationToken);
}
