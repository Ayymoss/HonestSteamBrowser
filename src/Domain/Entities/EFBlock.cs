using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Entities;

public class EFBlock
{
    [Key] public int Id { get; set; }
    public string Value { get; set; }
    public bool ApiFilter { get; set; }
    public FilterType Type { get; set; }
    public DateTimeOffset Added { get; set; }
    public int SteamGameId { get; set; }
    [ForeignKey(nameof(SteamGameId))] public EFSteamGame SteamGame { get; set; }

    public string UserId { get; set; }
}
