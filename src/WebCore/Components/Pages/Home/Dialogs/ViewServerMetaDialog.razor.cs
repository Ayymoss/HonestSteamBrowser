using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Dialogs;

public partial class ViewServerMetaDialog
{
    [Parameter] public required Server Server { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private List<PlayerInfo>? _players;
    private bool _isFavourite;
    private bool _processing;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;

        await IsServerFavouriteAsync(Server.IpAddress, Server.Port);
        StateHasChanged();

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(2000));

        try
        {
            _players = await Mediator.Send(new GetServerPlayersCommand
            {
                IpAddress = Server.IpAddress,
                Port = Server.Port
            }, cancellationTokenSource.Token) ?? new List<PlayerInfo>();
        }
        catch (OperationCanceledException)
        {
            _players = new List<PlayerInfo>();
        }

        StateHasChanged();
    }

    private async Task IsServerFavouriteAsync(string ipAddress, int port)
    {
        if (UserId is null) return;

        var result = await Mediator.Send(new IsServerFavouriteCommand
        {
            IpAddress = ipAddress,
            Port = port,
            UserId = UserId
        });
        _isFavourite = result;
    }

    private async Task AddToFavouriteAsync()
    {
        if (UserId is null) return;
        _processing = true;
        await Mediator.Publish(new ToggleFavouriteServerCommand
        {
            IpAddress = Server.IpAddress,
            Port = Server.Port,
            UserId = UserId
        });
        await IsServerFavouriteAsync(Server.IpAddress, Server.Port);
        _processing = false;
    }
}
