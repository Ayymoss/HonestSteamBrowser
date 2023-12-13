using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class BlockRepository(IDbContextFactory<DataContext> contextFactory) : IBlockRepository
{
    public async Task<List<EFBlock>> GetBlockListAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var blockList = await context.Blocks
            .Include(x => x.SteamGame)
            .ToListAsync(cancellationToken: cancellationToken);
        return blockList;
    }

    public async Task AddAsync(EFBlock block, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var alreadyExists = await context.Blocks
            .Where(x => x.Value == block.Value && x.SteamGameId == block.SteamGameId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (alreadyExists is not null) return;
        context.Blocks.Add(block);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var count = await context.Blocks.CountAsync(cancellationToken: cancellationToken);
        return count;
    }

    public async Task RemoveAsync(int blockId, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var block = await context.Blocks.FindAsync(new object?[] { blockId }, cancellationToken: cancellationToken);
        if (block is null) return;
        context.Blocks.Remove(block);
        await context.SaveChangesAsync(cancellationToken);
    }
}
