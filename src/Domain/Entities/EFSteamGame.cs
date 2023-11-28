using System.ComponentModel.DataAnnotations;

namespace BetterSteamBrowser.Domain.Entities;

public class EFSteamGame
{
    [Key] public int Id { get; set; }
    public int AppId { get; set; }
    public string Name { get; set; }
}
