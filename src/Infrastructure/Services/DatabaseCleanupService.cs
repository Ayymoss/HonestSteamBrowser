using BetterSteamBrowser.Domain.Interfaces.Repositories;
using BetterSteamBrowser.Domain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BetterSteamBrowser.Infrastructure.Services;

public class DatabaseCleanupService(
    IServerRepository serverRepository,
    IFavouriteRepository favouriteRepository,
    ISnapshotRepository snapshotRepository,
    ILogger<DatabaseCleanupService> logger)
    : IDatabaseCleanupService
{
    public async Task PurgeOldRecordsAsync(DateTimeOffset from, CancellationToken cancellationToken)
    {
        var serverHashes = await serverRepository.GetOlderServerHashesAsync(from, cancellationToken);
        logger.LogInformation("Found {ServerHashesCount} servers to purge", serverHashes.Count);
        await favouriteRepository.DeleteFavouritesByServerHashesAsync(serverHashes, cancellationToken);
        await serverRepository.DeleteServersByHashesAsync(serverHashes, cancellationToken);
        logger.LogInformation("Purged {ServerHashesCount} servers", serverHashes.Count);
        await snapshotRepository.DeleteSnapshotsByDateAsync(DateTimeOffset.UtcNow.AddYears(-1), cancellationToken);
        logger.LogInformation("Purged snapshots older than 1 year");
    }
}
