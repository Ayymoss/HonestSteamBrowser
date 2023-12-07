using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;

public partial class BlockServerDialog
{
    [Parameter] public required Server Server { get; set; }
    [Parameter] public required string UserId { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _forAllGames;
    private bool _processing;

    private async Task BlockAddress()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        await Mediator.Publish(new BlockServerAddressCommand {IpAddress = Server.IpAddress, SteamGameId = gameId, UserId = UserId});

        NotificationService.Notify(NotificationSeverity.Success, "IP Address blocked!");
        _processing = false;
        DialogService.Close();
    }
}
