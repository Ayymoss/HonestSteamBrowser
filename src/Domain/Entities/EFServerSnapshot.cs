using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServerSnapshot
{
    [Key] public int Id { get; set; }
    public int SnapshotCount { get; set; }
    public DateTimeOffset SnapshotTaken { get; set; }
    public string ServerHash { get; set; }
    [ForeignKey(nameof(ServerHash))] public EFServer Server { get; set; }
}
