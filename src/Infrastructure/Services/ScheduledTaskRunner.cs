using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BetterSteamBrowser.Infrastructure.Services;

public class ScheduledTaskRunner(IServiceProvider serviceProvider) : IDisposable
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
                await steamService.StartSteamFetchAsync(_cancellationTokenSource.Token);
                await databasePurgeService.PurgeOldRecordsAsync(DateTimeOffset.UtcNow.AddDays(-7), _cancellationTokenSource.Token);
                await publisher.Publish(new UpdateInformationCommand(), _cancellationTokenSource.Token);
                await statisticsService.FetchStatisticsAsync(_cancellationTokenSource.Token);
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
