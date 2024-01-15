using BetterSteamBrowser.Business.ViewModels;

namespace BetterSteamBrowser.Business.Services;

public interface IServerContextCache
{
    public int ServerCount { get; set; }
    public int PlayerCount { get; set; }
    public bool Loaded { get; set; }

    public void UpdateServerCount(CacheInfo cacheInfo)
    {
        ServerCount = cacheInfo.ServerCount;
        PlayerCount = cacheInfo.PlayerCount;
        Loaded = true;
    }
}
