using BetterSteamBrowser.Business.Mediatr.Events;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BetterSteamBrowser.WebCore.Components.Dialogs;

public partial class ManageAccountDialog
{
    [Parameter] public required string UserId { get; set; }
    [Inject] private NotificationService NotificationService { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private bool _processing;

    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;

    public void RedirectToAccount()
    {
        NavigationManager.NavigateTo("/Account", true);
    }

    private async Task ResetUserPasswordAsync()
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        if (_password != _confirmPassword)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Passwords do not match!");
            return;
        }

        if (_confirmPassword.Length < 6)
        {
            NotificationService.Notify(NotificationSeverity.Error, "Password must be at least 6 characters long!");
            return;
        }

        _processing = true;
        await Mediator.Publish(new ResetUserPasswordCommand {Id = UserId, Password = _confirmPassword});
        NotificationService.Notify(NotificationSeverity.Success, "Password changed!");
        _processing = false;
        DialogService.Close();
    }
}
