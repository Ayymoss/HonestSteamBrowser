using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServer
{
    [Key] public string Hash { get; set; }
    public string Address { get; set; }
    public string Name { get; set; }
    public int Players { get; set; }
    public int MaxPlayers { get; set; }
    public string? Country { get; set; }
    public string? CountryCode { get; set; }
    public string Map { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public bool Blacklisted { get; set; }

    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }
}
