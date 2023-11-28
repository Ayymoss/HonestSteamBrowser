namespace BetterSteamBrowser.Business.DTOs;

public class Server
{
    public string Address { get; set; }
    public string Name { get; set; }
    public string SteamGameName { get; set; }
    public int SteamGameAppId { get; set; }
    public string Map { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public string Country { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}
