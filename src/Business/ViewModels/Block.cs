using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Business.ViewModels;

public class Block
{
    public int Id { get; set; }
    public string Value { get; set; }
    public FilterType Type { get; set; }
    public DateTimeOffset Added { get; set; }
    public string SteamGameName { get; set; }
    public string AddedBy { get; set; }
    public bool ApiFilter { get; set; }
}
