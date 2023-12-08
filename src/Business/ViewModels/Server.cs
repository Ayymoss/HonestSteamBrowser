namespace BetterSteamBrowser.Business.ViewModels;

public class Server
{
    /// <summary>
    /// The IP Address. Excludes the port.
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// The Port Number.
    /// </summary>
    public int Port { get; set; }

    public string Name { get; set; }
    public int SteamGameId { get; set; }
    public string SteamGameName { get; set; }
    public int SteamGameAppId { get; set; }
    public string Map { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public string Country { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public bool Favourite { get; set; }

    /// <summary>
    /// Combination of IpAddress and Port.
    /// </summary>
    public string Address => $"{IpAddress}:{Port}";
}
