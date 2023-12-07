using BetterSteamBrowser.Domain.Entities;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface IBlockRepository
{
    Task<List<EFBlock>> GetBlockListAsync();
    Task AddAsync(EFBlock block, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task RemoveAsync(int blockId, CancellationToken cancellationToken);
}
