using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.WebCore.Components.Pages.Home.Dialogs;
using BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;
using BetterSteamBrowser.WebCore.Utilities;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using Radzen.Blazor;
using SortDescriptor = BetterSteamBrowser.Domain.ValueObjects.Pagination.SortDescriptor;

namespace BetterSteamBrowser.WebCore.Components.Pages.Home.Subcomponents;

public partial class ServerList : IDisposable
{
    [Parameter] public bool Manager { get; set; }
    [Parameter] public bool IsAdmin { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private TooltipService TooltipService { get; set; }

    private RadzenDataGrid<Server> _dataGrid;
    private IEnumerable<Server> _serverTable;
    private IEnumerable<SteamGame> _dropDownGames;
    private SteamGame? _dropDownSelected;
    private bool _isLoading = true;
    private int _count;
    private int _gamePlayerCount;
    private string? _searchString;
    private string _titleText = "Servers";
    private bool _showFavourites;

    protected override async Task OnInitializedAsync()
    {
        var steamGames = await Mediator.Send(new GetSteamGamesCommand());
        _dropDownGames = steamGames.OrderBy(x => x.Name);

        var uri = new Uri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        query.TryGetValue("game", out var game);
        query.TryGetValue("filter", out var filter);
        _dropDownSelected = int.TryParse(game.FirstOrDefault(), out var gameAppId)
            ? _dropDownGames.FirstOrDefault(x => x.AppId == gameAppId)
            : null;
        _searchString = filter.FirstOrDefault();
        await base.OnInitializedAsync();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;

        var paginationQuery = new GetServerListCommand
        {
            Sorts = args.Sorts.Select(x => new SortDescriptor
            {
                Property = x.Property,
                SortOrder = x.SortOrder == SortOrder.Ascending
                    ? SortDirection.Ascending
                    : SortDirection.Descending
            }),
            SearchString = _searchString,
            Data = _dropDownSelected?.AppId,
            Top = args.Top ?? 20,
            Skip = args.Skip ?? 0,
            UserId = UserId,
            Favourites = _showFavourites
        };

        var context = await Mediator.Send(paginationQuery);
        _serverTable = context.Data;
        _count = context.Count;
        _gamePlayerCount = context.Players;
        _isLoading = false;
        UpdateTitle();
    }

    private async Task OnDropdownChanged()
    {
        await _dataGrid.Reload();
        UpdateUrl();
    }

    private async Task OnSearch(string text)
    {
        _searchString = text;
        await _dataGrid.Reload();
        UpdateUrl();
    }

    private void UpdateTitle()
    {
        _titleText = _dropDownSelected is not null ? $"Players {_gamePlayerCount:N0}" : "Servers";
        StateHasChanged();
    }

    private void UpdateUrl()
    {
        var baseUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path);

        var queryString = new Dictionary<string, string?>();
        if (_dropDownSelected is not null)
        {
            queryString["game"] = _dropDownSelected.AppId.ToString();
        }

        if (!string.IsNullOrEmpty(_searchString))
        {
            queryString["filter"] = _searchString;
        }

        var newUri = QueryHelpers.AddQueryString(baseUri, queryString);
        NavigationManager.NavigateTo(newUri, false);
    }

    public void Dispose()
    {
        _dataGrid.Dispose();
    }

    private async Task RowClickEvent(DataGridRowMouseEventArgs<Server> arg)
    {
        var parameters = new Dictionary<string, object>
        {
            {"Server", arg.Data},
            {"UserId", UserId ?? string.Empty}
        };

        var options = new DialogOptions
        {
            Style = "min-height:auto;min-width:auto;width:auto;max-width:75%;max-height:97%",
            CloseDialogOnOverlayClick = true
        };

        if (Manager)
        {
            if (!IsAdmin) return;

            await DialogService.OpenAsync<BlacklistServerDialog>("Blacklist Address?", parameters, options);
            await _dataGrid.Reload();
        }
        else
        {
            await DialogService.OpenAsync<ViewServerMetaDialog>("Server Meta", parameters, options);
            await _dataGrid.Reload();
        }
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options, string message) =>
        TooltipService.Open(elementReference, message, options);

    private async Task OnFavouriteClick(bool favourites)
    {
        _showFavourites = favourites;
        await _dataGrid.Reload();
    }
}
