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
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string Map { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Blocked { get; set; }

    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }
}
