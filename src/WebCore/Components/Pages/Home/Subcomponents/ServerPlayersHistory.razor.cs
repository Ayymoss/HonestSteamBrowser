using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;

public partial class ServerPlayersHistory
{
    [Parameter] public Server Server { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _loading;

    protected override async Task OnInitializedAsync()
    {
        if (Server.ServerSnapshots is not null) return;

        _loading = true;
        Server.ServerSnapshots = await Mediator.Send(new GetServerSnapshotsCommand {Hash = Server.Hash});
        _loading = false;
    }

    private string DateFormatter(object arg)
    {
        if (arg is DateTime dateTime) return dateTime.ToString("MM-dd HH:mm");
        return string.Empty;
    }
}
