using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;

public partial class BlacklistServerDialog
{
    [Parameter] public required Server Server { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _forAllGames;
    private bool _processing;

    private async Task BlacklistAddress()
    {
        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        await Mediator.Publish(new BlacklistServerAddressCommand {IpAddress = Server.IpAddress, SteamGameId = gameId});

        NotificationService.Notify(NotificationSeverity.Success, "IP Address Blacklisted!");
        _processing = false;
        DialogService.Close();
    }
}
