using BetterSteamBrowser.Business.DTOs;

namespace BetterSteamBrowser.Business.Services;

public class ServerContextCache
{
    public int ServerCount { get; private set; }
    public int PlayerCount { get; private set; }
    public bool Loaded { get; private set; }

    public void UpdateServerCount(CacheInfo cacheInfo)
    {
        ServerCount = cacheInfo.ServerCount;
        PlayerCount = cacheInfo.PlayerCount;
        Loaded = true;
    }
}
