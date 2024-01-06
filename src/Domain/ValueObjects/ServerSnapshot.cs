namespace BetterSteamBrowser.Domain.ValueObjects;

public class ServerSnapshot
{
    public int Count { get; set; }
    public DateTimeOffset Snapshot { get; set; }
}
