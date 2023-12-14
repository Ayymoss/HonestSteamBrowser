using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories;

public class SnapshotRepository(IDbContextFactory<DataContext> contextFactory) : ISnapshotRepository
{
    public async Task SubmitSnapshotAsync(SnapshotType snapshotType, int count, DateTimeOffset dateTimeOffset,
        CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var snapshot = new EFSnapshot
        {
            Snapshot = snapshotType,
            Count = count,
            Stored = dateTimeOffset
        };
        context.Snapshots.Add(snapshot);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSnapshotsByDateAsync(DateTimeOffset from, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var snapshots = await context.Snapshots
            .Where(x => x.Stored < from)
            .ToListAsync(cancellationToken: cancellationToken);
        context.Snapshots.RemoveRange(snapshots);
        await context.SaveChangesAsync(cancellationToken);
    }
}
