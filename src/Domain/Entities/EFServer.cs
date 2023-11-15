using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServer
{
    [Key] public string Hash { get; set; }
    public string Address { get; set; }
    public string Name { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public SteamGame Game { get; set; }
    public Region Region { get; set; }
    public string Map { get; set; }

    public DateTimeOffset LastUpdated { get; set; }
}
