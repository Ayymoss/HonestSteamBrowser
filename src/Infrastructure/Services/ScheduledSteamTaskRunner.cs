using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Domain.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BetterSteamBrowser.Infrastructure.Services;

public class ScheduledSteamTaskRunner(IServiceProvider serviceProvider) : IDisposable
{
    private Timer? _timer;
    private bool _firstRun = true;

    public void StartTimer()
    {
        _timer = new Timer(ExecuteScheduledAction, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
    }

    private void ExecuteScheduledAction(object? state)
    {
#if !DEBUG
        if (_firstRun)
        {
            _firstRun = false;
            return;
        }
#endif

        var cancellationTokenSource = new CancellationTokenSource();

        Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            var steamService = scope.ServiceProvider.GetRequiredService<ISteamServerService>();
            var databasePurgeService = scope.ServiceProvider.GetRequiredService<IDatabaseCleanupService>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            try
            {
                await steamService.StartSteamFetchAsync(cancellationTokenSource.Token);
                await databasePurgeService.PurgeOldRecordsAsync(DateTimeOffset.UtcNow.AddDays(-7), cancellationTokenSource.Token);
                await publisher.Publish(new UpdateInformationCommand(), cancellationTokenSource.Token);
            }
            catch (Exception e)
            {
                Log.Error(e, "Error executing scheduled action");
            }
        }, cancellationTokenSource.Token);
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
