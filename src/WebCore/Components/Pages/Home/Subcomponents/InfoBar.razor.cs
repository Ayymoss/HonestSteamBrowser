using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
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

    private int _favouriteCount;
    private int _blacklistCount;
    private bool _loading = true;

    private bool _showFavourites;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender) return;
        _loading = true;
        if (UserId is null) return;

        if (IsAdmin) _blacklistCount = await Mediator.Send(new GetBlacklistCountCommand());
        _favouriteCount = await Mediator.Send(new GetUserFavouriteCountCommand {UserId = UserId});
        _loading = false;
        StateHasChanged();
    }

    private Task OnFavouriteClick()
    {
        if (WebContext is null) return Task.CompletedTask;
        WebContext.IsFavouriteChecked = _showFavourites;
        return OnWebContextUpdated.InvokeAsync(WebContext);
    }
}
