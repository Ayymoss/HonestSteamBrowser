using System.ComponentModel.DataAnnotations;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Subcomponents;

public partial class AddBlock
{
    [Parameter] public string? UserId { get; set; }
    [Parameter] public EventCallback OnBlockCreated { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }


    private IEnumerable<SteamGame> _dropDownGames;
    private BlockOptions _blockOptions = new();
    private bool _processing;

    protected override async Task OnInitializedAsync()
    {
        var steamGames = await Mediator.Send(new GetSteamGamesCommand());
        _dropDownGames = steamGames.OrderBy(x => x.Name);
        await base.OnInitializedAsync();
    }

    private async Task CreateBlockAsync(BlockOptions args)
    {
        if (string.IsNullOrWhiteSpace(args.Value)) return;
        if (string.IsNullOrWhiteSpace(UserId)) return;

        _processing = true;
        var command = new CreateBlockCommand
        {
            UserId = UserId,
            SteamGameId = args.FilterAllGames ? SteamGameConstants.AllGames : args.SteamGame.AppId,
            Value = args.Value,
            Type = args.FilterType,
            ApiFilter = args.ApiFilter,
        };
        await Mediator.Publish(command);
        await OnBlockCreated.InvokeAsync();
        NotificationService.Notify(NotificationSeverity.Success, "Created block!");
        _processing = false;
    }

    public class BlockOptions
    {
        public string? Value { get; set; }
        public bool ApiFilter { get; set; }
        public FilterType FilterType { get; set; }
        public SteamGame SteamGame { get; set; }
        public bool FilterAllGames { get; set; }
    }
}
