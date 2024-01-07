using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;

public partial class ServerPlayersHistory
{
    private record Snapshot(DateTime DateTime, int Count);

    [Parameter] public Server Server { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _loading;

    protected override void OnInitialized()
    {
        if (Server.ServerSnapshots is not null) return;

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(2000));

        _loading = true;

        _ = Task.Run(async () =>
        {
            var snapshots = await Mediator.Send(new GetServerSnapshotsCommand {Hash = Server.Hash}, cancellationTokenSource.Token);

            await InvokeAsync(() =>
            {
                Server.ServerSnapshots = snapshots;
                _loading = false;
                StateHasChanged();
            });
        }, cancellationTokenSource.Token);
    }

    private string DateFormatter(object arg)
    {
        if (arg is DateTime dateTime) return dateTime.ToString("MM-dd HH:mm");
        return string.Empty;
    }
}
