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
        // TODO: Move the save period to cache then write out cache every 6 hours
        _timer = new Timer(ExecuteScheduledAction, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
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

        Task.Run(async () =>
        {
            using var scope = serviceProvider.CreateScope();
            var steamService = scope.ServiceProvider.GetRequiredService<ISteamServerService>();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            try
            {
                await steamService.StartSteamFetchAsync();
                await publisher.Publish(new UpdateInformationCommand());
            }
            catch (Exception e)
            {
                Log.Error(e, "Error executing scheduled action");
            }
        });
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
