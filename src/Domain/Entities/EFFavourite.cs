using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetterSteamBrowser.Domain.Entities;

public class EFFavourite
{
    [Key] public int Id { get; set; }
    public string ServerId { get; set; }
    [ForeignKey(nameof(ServerId))] public EFServer Server { get; set; }
    public string UserId { get; set; }
}
