using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Dialogs;

public partial class ServerDialog
{
    [Parameter] public required Server Server { get; set; }
    [Inject] private NotificationService notificationService { get; set; }
    [Inject] private DialogService dialogService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _forAllGames;
    private bool _processing;

    private async Task BlacklistAddress()
    {
        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        await Mediator.Publish(new BlacklistServerAddressCommand {IpAddress = Server.IpAddress, SteamGameId = gameId});

        notificationService.Notify(NotificationSeverity.Success, "IP Address Blacklisted!");
        _processing = false;
        dialogService.Close();
    }
}
