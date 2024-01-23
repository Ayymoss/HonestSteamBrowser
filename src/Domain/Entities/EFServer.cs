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
    public double? PlayersStandardDeviation { get; set; }

    public double? PlayerAverage { get; set; }
    public int? PlayerUpperBound { get; set; }
    public int? PlayerLowerBound { get; set; }

    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string? AutonomousSystemOrganization { get; set; }
    public string Map { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Blocked { get; set; }

    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }

    /// <summary>
    /// Navigation property for the server snapshots.
    /// </summary>
    public List<EFServerSnapshot>? Snapshots { get; set; }

    [NotMapped] public static int OldestPlayerSnapshotInDays { get; } = 7;

    public void UpdateServerStatistics()
    {
        // Get the window for the EMA
        var deltaDays = Math.Round((DateTimeOffset.UtcNow - DateTimeOffset.UtcNow.AddDays(-OldestPlayerSnapshotInDays)).TotalDays);
        var maxCount = (double)60 / 15 * 24 * deltaDays;
        var smoothingFactor = 2.0 / (maxCount + 1);
        PlayerAverage = PlayerAverage.HasValue
            ? Players * smoothingFactor + PlayerAverage.Value * (1 - smoothingFactor)
            : 0;
        PlayerUpperBound = !PlayerUpperBound.HasValue ? Players : Players > PlayerUpperBound ? Players : PlayerUpperBound;
        PlayerLowerBound = !PlayerLowerBound.HasValue ? Players : Players < PlayerLowerBound ? Players : PlayerLowerBound;
        Snapshots = [new EFServerSnapshot {ServerHash = Hash, PlayerCount = Players, Taken = DateTimeOffset.UtcNow}];
    }

    public void BlockServer()
    {
        Blocked = true;
        Snapshots = [];
    }
}
