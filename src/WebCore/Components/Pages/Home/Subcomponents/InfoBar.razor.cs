using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.WebCore.Utilities;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using IdentityRole = BetterSteamBrowser.Domain.Enums.IdentityRole;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;

public partial class InfoBar
{
    [CascadingParameter] public WebInfoContext? WebContext { get; set; }
    [Parameter] public bool IsAdmin { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Parameter] public EventCallback<WebInfoContext> OnWebContextUpdated { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private int _favouriteCount;
    private int _blockCount;
    private bool _showFavourites;

    private static int SnapshotCount => Enum.GetNames(typeof(SnapshotType)).Length;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        if (UserId is null) return;
        if (IsAdmin) _blockCount = await Mediator.Send(new GetBlockCountCommand());
        _favouriteCount = await Mediator.Send(new GetUserFavouriteCountCommand {UserId = UserId});
        StateHasChanged();
    }

    private Task OnFavouriteClick()
    {
        if (WebContext is null) return Task.CompletedTask;
        WebContext.IsFavouriteChecked = _showFavourites;
        return OnWebContextUpdated.InvokeAsync(WebContext);
    }

    public void RedirectToAccount()
    {
        NavigationManager.NavigateTo("/Account", true);
    }
}
