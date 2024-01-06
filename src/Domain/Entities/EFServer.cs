using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServer
{
    [Key] public string Hash { get; set; }

    /// <summary>
    /// The IP Address. Excludes the port.
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// The Port number.
    /// </summary>
    public int Port { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Current number of players on the server.
    /// </summary>
    public int Players { get; set; }

    /// <summary>
    /// Maximum number of players allowed on the server.
    /// </summary>
    public int MaxPlayers { get; set; }

    /// <summary>
    /// The standard deviation of the player count. This is used to detect spoofed servers.
    /// </summary>
    public double? PlayersStandardDeviation { get; private set; }

    public double? PlayerAverage { get; set; }
    public int? PlayerUpperBound { get; set; }
    public int? PlayerLowerBound { get; set; }

    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string Map { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Blocked { get; set; }

    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }

    /// <summary>
    /// Navigation property for the server snapshots.
    /// </summary>
    public List<EFServerSnapshot>? ServerSnapshots { get; set; }

    public void AddToHistory()
    {
        ServerSnapshots ??= [];
        ServerSnapshots.Add(new EFServerSnapshot {SnapshotCount = Players, SnapshotTaken = DateTimeOffset.UtcNow});
        PlayerAverage = ServerSnapshots.Average(x => x.SnapshotCount);
        PlayerUpperBound = ServerSnapshots.Min(x => x.SnapshotCount);
        PlayerLowerBound = ServerSnapshots.Max(x => x.SnapshotCount);

        // Cap the history - 1 Week ((60 / 4) * 24 * 7)
        if (ServerSnapshots.Count > 672) ServerSnapshots.RemoveAt(0);
    }

    public void BlockServer()
    {
        Blocked = true;
        ServerSnapshots = [];
    }

    public void UpdateStandardDeviation()
    {
        if (!(ServerSnapshots?.Count > 1)) return;

        var avg = ServerSnapshots.Average(x => x.SnapshotCount);
        var sumOfSquaresOfDifferences = ServerSnapshots.Sum(x => (x.SnapshotCount - avg) * (x.SnapshotCount - avg));
        var standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / ServerSnapshots.Count);
        PlayersStandardDeviation = avg > 0 ? standardDeviation / avg : 0;
    }
}
