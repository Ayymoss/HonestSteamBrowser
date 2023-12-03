namespace BetterSteamBrowser.Domain.ValueObjects;

public class PlayerInfo
{
    public string? Name { get; set; }
    public long? Score { get; set; }
    public TimeSpan Online { get; set; }
}
