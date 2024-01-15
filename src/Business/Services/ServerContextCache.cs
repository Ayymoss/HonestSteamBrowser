namespace BetterSteamBrowser.Business.Services;

public class ServerContextCache : IServerContextCache
{
    public int ServerCount { get; set; }
    public int PlayerCount { get; set; }
    public bool Loaded { get; set; }
}