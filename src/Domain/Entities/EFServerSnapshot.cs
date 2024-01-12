using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterSteamBrowser.Domain.Entities;

public class EFServerSnapshot
{
    [Key] public int Id { get; set; }
    public int PlayerCount { get; set; }
    public DateTimeOffset Taken { get; set; }
    public string ServerHash { get; set; }
    [ForeignKey(nameof(ServerHash))] public EFServer Server { get; set; }
}
