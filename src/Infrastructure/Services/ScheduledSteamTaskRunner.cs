﻿using BetterSteamBrowser.Business.Mediatr.Events;
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
        _timer = new Timer(ExecuteScheduledAction, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    private void ExecuteScheduledAction(object? state)
    {
        if (_firstRun)
        {
            _firstRun = false;
            return;
        }

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
