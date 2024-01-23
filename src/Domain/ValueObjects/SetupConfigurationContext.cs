namespace BetterSteamBrowser.Domain.ValueObjects;

public class SetupConfigurationContext
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string SteamApiKey { get; set; }
    public int WebBind { get; set; } = 443;
    public string MaxMindGeoIp2DatabaseName { get; set; } = "GeoLite2-Country.mmdb";
    public string MaxMindGeoIp2AsnDatabaseName { get; set; } = "GeoLite2-ASN.mmdb";
    public string? CertificatePath { get; set; }
    public string? CertificatePassword { get; set; }
}
