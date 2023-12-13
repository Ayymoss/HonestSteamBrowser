namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface IDatabaseCleanupService
{
    Task PurgeOldRecordsAsync(DateTimeOffset from, CancellationToken cancellationToken);
}
