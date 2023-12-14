namespace BetterSteamBrowser.Domain.Interfaces.Services;

public interface IStatisticsService
{
    Task FetchStatisticsAsync(CancellationToken cancellationToken);
}
