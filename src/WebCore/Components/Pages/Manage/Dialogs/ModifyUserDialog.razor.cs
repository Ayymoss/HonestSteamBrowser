using BetterSteamBrowser.Business.Mediatr.Events;
using BetterSteamBrowser.Business.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;

public partial class ModifyUserDialog
{
    [Parameter] public required User User { get; set; }
    [Parameter] public required string UserId { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

    private bool _processing;
    private bool _isAdmin;

    private async Task ModifyUserAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        _processing = true;
        await Mediator.Publish(new ChangeUserRoleCommand {Id = User.Id, IsAdmin = _isAdmin});
        NotificationService.Notify(NotificationSeverity.Success, "User modified!");
        _processing = false;
        DialogService.Close();
    }

    private async Task ResetUserPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        _processing = true;
        await Mediator.Publish(new ResetUserPasswordCommand {Id = User.Id});
        NotificationService.Notify(NotificationSeverity.Success, "Password reset!");
        _processing = false;
        DialogService.Close();
    }
}
