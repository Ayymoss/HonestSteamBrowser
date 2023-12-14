using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.Entities;

public class EFSnapshot
{
    [Key] public int Id { get; set; }
    public SnapshotType Snapshot { get; set; }
    public int Count { get; set; }
    public DateTimeOffset Stored { get; set; }
}
