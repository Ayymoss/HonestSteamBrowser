namespace BetterSteamBrowser.Domain.ValueObjects;

public class AsnPreBlock
{
    public int Players { get; set; }
    public string IpAddress { get; set; }
    public int Port { get; set; }
    public string Name { get; set; }
    public int SteamGameId { get; set; }
    public int MaxPlayers { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset Created { get; set; }
    public string? CountryCode { get; set; }
    public double? PlayersStandardDeviation { get; set; }
    public double PlayerAverage { get; set; }
    public int PlayerUpper { get; set; }
    public int PlayerLower { get; set; }
    public string Address => $"{IpAddress}:{Port}";
}
