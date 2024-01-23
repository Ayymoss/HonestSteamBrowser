using BetterSteamBrowser.Domain.ValueObjects;

namespace BetterSteamBrowser.Business.ViewModels;

public class Server
{
    public string Hash { get; set; }

    /// <summary>
    /// The IP Address. Excludes the port.
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// The Port Number.
    /// </summary>
    public int Port { get; set; }

    public string Name { get; set; }
    public string? AutonomousSystemOrganization { get; set; }
    public int SteamGameId { get; set; }
    public string SteamGameName { get; set; }
    public string Map { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public List<ServerSnapshot>? ServerSnapshots { get; set; }
    public double? PlayersStandardDeviation { get; set; }
    public string Country { get; set; }
    public string? CountryCode { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Favourite { get; set; }
    public double PlayerAverage { get; set; }
    public int PlayerUpper { get; set; }
    public int PlayerLower { get; set; }

    /// <summary>
    /// Combination of IpAddress and Port.
    /// </summary>
    public string Address => $"{IpAddress}:{Port}";
}
