using RestEase;

namespace BetterSteamBrowser.Infrastructure.Interfaces;

public interface ISteamApi
{
    [Get("IGameServersService/GetServerList/v1/")]
    Task<HttpResponseMessage> GetServerListAsync([QueryMap] Dictionary<string, string> queryParams);
}
