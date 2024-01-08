using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServer
{
    /// <summary>
    /// Cap the history - 1 Week ((60 / 15) * 24 * 7)
    /// </summary>
    public const int SnapshotMax = 672;

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
    public double? PlayersStandardDeviation { get; set; }

    public double? PlayerAverage { get; set; }
    public int? PlayerUpperBound { get; set; }
    public int? PlayerLowerBound { get; set; }
    public int? PlayerSnapshotCount { get; set; }

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

    public void UpdateServerStatistics()
    {
        ServerSnapshots ??= [];
        ServerSnapshots.Add(new EFServerSnapshot {ServerHash = Hash, SnapshotCount = Players, SnapshotTaken = DateTimeOffset.UtcNow});
        PlayerSnapshotCount = !PlayerSnapshotCount.HasValue
            ? 1
            : PlayerSnapshotCount < SnapshotMax
                ? PlayerSnapshotCount++
                : PlayerSnapshotCount;
        PlayerAverage = PlayerAverage.HasValue ? PlayerAverage + (Players - PlayerAverage) / PlayerSnapshotCount : Players;
        PlayerUpperBound = !PlayerUpperBound.HasValue ? Players : Players > PlayerUpperBound ? Players : PlayerUpperBound;
        PlayerLowerBound = !PlayerLowerBound.HasValue ? Players : Players < PlayerLowerBound ? Players : PlayerLowerBound;
    }

    public void BlockServer()
    {
        Blocked = true;
        ServerSnapshots = [];
    }
}
