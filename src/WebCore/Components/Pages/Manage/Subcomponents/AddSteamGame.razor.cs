using BetterSteamBrowser.Business.Mediatr.Events;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Subcomponents;

public partial class AddSteamGame
{
    [Parameter] public string? UserId { get; set; }
    [Parameter] public EventCallback OnSteamGameCreated { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }

    private SteamGameOptions _steamGameOptions = new();
    private bool _processing;

    private async Task CreateSteamGameAsync(SteamGameOptions args)
    {
        if (args.AppId < 1) return;
        if (string.IsNullOrWhiteSpace(args.Name)) return;
        if (string.IsNullOrWhiteSpace(UserId)) return;

        _processing = true;
        var command = new CreateSteamGameCommand
        {
            AppId = args.AppId,
            Name = args.Name,
        };
        await Mediator.Publish(command);
        await OnSteamGameCreated.InvokeAsync();
        NotificationService.Notify(NotificationSeverity.Success, "Added new Steam Game!");
        _processing = false;
    }

    public class SteamGameOptions
    {
        public int AppId { get; set; }
        public string Name { get; set; }
    }
}
