using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories;

public interface ISnapshotRepository
{
    Task SubmitSnapshotAsync(SnapshotType snapshotType, int count, DateTimeOffset dateTimeOffset, CancellationToken cancellationToken);
    Task DeleteSnapshotsByDateAsync(DateTimeOffset from, CancellationToken cancellationToken);
}
