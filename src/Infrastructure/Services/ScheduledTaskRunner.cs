using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BetterSteamBrowser.Infrastructure.Services;

public class ScheduledTaskRunner(IServiceProvider serviceProvider, ILogger<ScheduledTaskRunner> logger) : IDisposable
{
    private Timer? _steamTimer;
    private bool _steamFirstRun = true;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public void StartTimer()
    {
        _steamTimer = new Timer(ScheduleSteamActions, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
    }

    private void ScheduleSteamActions(object? state)
    {
#if !DEBUG
        if (_steamFirstRun)
        {
            _steamFirstRun = false;
            return;
        }
#endif

        Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            var steamService = scope.ServiceProvider.GetRequiredService<ISteamServerService>();
            var databasePurgeService = scope.ServiceProvider.GetRequiredService<IDatabaseCleanupService>();
            var statisticsService = scope.ServiceProvider.GetRequiredService<IStatisticsService>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            try
            {
                logger.LogInformation("Fetching Steam Server List");
                await steamService.StartSteamFetchAsync(_cancellationTokenSource.Token);

                logger.LogInformation("Purging old records");
                await databasePurgeService.PurgeOldRecordsAsync(DateTimeOffset.UtcNow.AddDays(-7), _cancellationTokenSource.Token);

                logger.LogInformation("Updating statistics cache");
                await publisher.Publish(new UpdateInformationCommand(), _cancellationTokenSource.Token);

                logger.LogInformation("Updating statistics page");
                await statisticsService.FetchStatisticsAsync(_cancellationTokenSource.Token);

                logger.LogInformation("Finished scheduled action");
            }
            catch (Exception e)
            {
                Log.Error(e, "Error executing scheduled action");
            }
        }, _cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        _steamTimer?.Dispose();
        _cancellationTokenSource.Cancel();
    }
}
