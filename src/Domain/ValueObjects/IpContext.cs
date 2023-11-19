namespace BetterSteamBrowser.Domain.ValueObjects;

public class IpContext
{
    public string IpAddress { get; set; }
    public string? CountryCode { get; set; }
    public string? Country { get; set; }
}
