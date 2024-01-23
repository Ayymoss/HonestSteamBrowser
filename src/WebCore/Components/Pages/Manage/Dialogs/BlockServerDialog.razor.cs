using System.Net;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Services;
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
    private string? _checkResult;
    private IEnumerable<AsnPreBlock> _asnPreBlocks = [];
    private AutonomousSystemData? _asnData;

    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;

        _asnData = await Mediator.Send(new GetAsnDataCommand
        {
            IpAddress = IPAddress.Parse(Server.IpAddress)
        });

        await base.OnInitializedAsync();
    }

    private async Task BlockAddress()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;

        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        await Mediator.Publish(new BlockServerAddressCommand {IpAddress = Server.IpAddress, SteamGameId = gameId, UserId = UserId});
        NotificationService.Notify(NotificationSeverity.Success, "IP Address blocked!");

        _processing = false;
        DialogService.Close(true);
    }

    private async Task AsnBlockCheck()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        if (string.IsNullOrWhiteSpace(_asnData?.AutonomousSystemOrganization)) return;

        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        var servers = await Mediator.Send(new CheckAsnCountsCommand
        {
            AutonomousSystemOrganization = _asnData.AutonomousSystemOrganization,
            SteamGameId = gameId
        });
        _checkResult = $"{_asnData.AutonomousSystemOrganization} -> " +
                       $"Servers: {servers.Count:N0}, " +
                       $"Players {servers.Sum(x => x.Players):N0}";

        _asnPreBlocks = servers.OrderByDescending(x => x.Players);

        _processing = false;
    }

    private async Task BlockAsn()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        if (string.IsNullOrWhiteSpace(_asnData?.AutonomousSystemOrganization)) return;

        _processing = true;

        var gameId = _forAllGames ? SteamGameConstants.AllGames : Server.SteamGameId;
        await Mediator.Publish(new BlockAsnCommand
            {AutonomousSystemOrganization = _asnData.AutonomousSystemOrganization, SteamGameId = gameId, UserId = UserId});
        NotificationService.Notify(NotificationSeverity.Success, "ASN blocked!");

        _processing = false;
        DialogService.Close(true);
    }
}
