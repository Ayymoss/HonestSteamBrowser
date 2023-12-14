using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.ValueObjects;
using ClipLazor.Components;
using ClipLazor.Enums;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Dialogs;

public partial class ViewServerMetaDialog
{
    [Parameter] public required Server Server { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IClipLazor Clipboard { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }

    private List<PlayerInfo>? _players;
    private bool _isFavourite;
    private bool _processing;
    private bool _isClipboardSupported;

    private string SteamUrl => $"steam://connect/{Server.Address}";

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        _isClipboardSupported = await Clipboard.IsPermitted(PermissionCommand.Write);

        await IsServerFavouriteAsync(Server.IpAddress, Server.Port);
        StateHasChanged();

        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(TimeSpan.FromMilliseconds(2000));

        _players = await Mediator.Send(new GetServerPlayersCommand
        {
            IpAddress = Server.IpAddress,
            Port = Server.Port
        }, cancellationTokenSource.Token) ?? [];

        StateHasChanged();
    }

    private async Task CopyToClipboardAsync()
    {
        if (!_isClipboardSupported) return;
        await Clipboard.WriteTextAsync(Server.Address.AsMemory());
        NotificationService.Notify(NotificationSeverity.Info, "Copied IP to Clipboard", Server.Address);
    }

    public void ConnectToServer()
    {
        NavigationManager.NavigateTo(SteamUrl, true);
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
