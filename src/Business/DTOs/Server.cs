using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Business.DTOs;

public class Server
{
    public string Address { get; set; }
    public string Name { get; set; }
    public SteamGame Game { get; set; }
    public string Map { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public Region Region { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
}
