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
    /// The historical number of players on the server.
    /// </summary>
    public List<int>? PlayerHistory { get; private set; } = [];

    /// <summary>
    /// The lowest amount of players seen on the server. This is used to detect spoofed servers.
    /// </summary>
    public int LowerBoundPlayers { get; private set; }

    /// <summary>
    /// The highest amount of players seen on the server. This is used to detect spoofed servers.
    /// </summary>
    public int UpperBoundPlayers { get; private set; }

    /// <summary>
    /// The standard deviation of the player count. This is used to detect spoofed servers.
    /// </summary>
    public double? PlayerStandardDeviation { get; set; }
    
    /// <summary>
    /// The standard global deviation ratio for the game context.
    /// </summary>
    public double? PlayerGlobalStandardDeviationRatio { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string Map { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Blocked { get; set; }

    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }

    public void AddToHistory(int newHistoryItem)
    {
        PlayerHistory?.Add(newHistoryItem);
        // Cap the history at 25 items
        if (PlayerHistory?.Count > 25) PlayerHistory.RemoveAt(0);
    }

    public void UpdateBounds(int players)
    {
        if (LowerBoundPlayers is 0 && UpperBoundPlayers is 0)
        {
            LowerBoundPlayers = players;
            UpperBoundPlayers = players;
        }

        if (players < LowerBoundPlayers) LowerBoundPlayers = players;
        if (players > UpperBoundPlayers) UpperBoundPlayers = players;
    }
}
