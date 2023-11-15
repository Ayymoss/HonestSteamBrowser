using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Entities;

public class EFBlacklist
{
    [Key] public int Id { get; set; }
    public string Value { get; set; }
    public bool ApiFilter { get; set; }
    public SteamGame Game { get; set; }
    public BlacklistType Type { get; set; }
    public DateTimeOffset Added { get; set; }
}
