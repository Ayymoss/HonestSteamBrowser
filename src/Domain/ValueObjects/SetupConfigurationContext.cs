namespace BetterSteamBrowser.Domain.ValueObjects;

public class SetupConfigurationContext
{
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string SteamApiKey { get; set; }
}
