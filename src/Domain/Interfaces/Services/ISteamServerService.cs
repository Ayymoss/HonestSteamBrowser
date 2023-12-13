namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface ISteamServerService
{
    Task StartSteamFetchAsync(CancellationToken cancellationToken);
}
