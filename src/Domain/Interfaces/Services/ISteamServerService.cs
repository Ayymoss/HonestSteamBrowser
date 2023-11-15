using BetterSteamBrowser.Domain.ValueObjects;

namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface ISteamServerService
{
    Task StartSteamFetchAsync();
}
