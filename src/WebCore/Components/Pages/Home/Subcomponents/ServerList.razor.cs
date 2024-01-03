using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
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
    [CascadingParameter] public WebInfoContext? WebContext { get; set; }
    [Parameter] public bool Manager { get; set; }
    [Parameter] public bool IsAdmin { get; set; }
    [Parameter] public string? UserId { get; set; }
    [Parameter] public EventCallback OnServerBlockCreated { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private DialogService DialogService { get; set; }
    [Inject] private TooltipService TooltipService { get; set; }

    private RadzenDataGrid<Server> _dataGrid;
    private IEnumerable<Server> _serverTable;

    private List<SteamGame> _gamesFilterList = [];
    private IEnumerable<SteamGame> _gamesFilterData;
    private SteamGame? _gamesFilterSelected;

    private IEnumerable<string> _regionFilterData = ["Africa", "Americas", "Asia", "Europe", "Oceania"];
    private string? _regionFilterSelected;

    private bool _isLoading = true;
    private int _count;
    private int _gamePlayerCount;
    private string? _searchString;
    private string _titleText = "Servers";

    private IEnumerable<int> PageSizes => Manager ? [25, 50, 100, 500] : [25, 50, 100];

    protected override async Task OnInitializedAsync()
    {
        var steamGames = await Mediator.Send(new GetSteamGamesCommand());
        _gamesFilterList = steamGames.OrderBy(x => x.Name).ToList();
        _gamesFilterData = _gamesFilterList;

        var uri = new Uri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        query.TryGetValue("region", out var region);
        query.TryGetValue("game", out var game);
        query.TryGetValue("search", out var search);
        _regionFilterSelected = region.FirstOrDefault();
        _gamesFilterSelected = int.TryParse(game.FirstOrDefault(), out var gameAppId)
            ? _gamesFilterList.FirstOrDefault(x => x.AppId == gameAppId)
            : null;
        _searchString = search.FirstOrDefault();
        await base.OnInitializedAsync();
    }

    private async Task TableLoadData(LoadDataArgs args)
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
            Search = _searchString,
            Top = args.Top ?? 20,
            Skip = args.Skip ?? 0,
            UserId = UserId,
            Favourites = WebContext?.IsFavouriteChecked ?? false,
            AppId = _gamesFilterSelected?.AppId,
            Region = _regionFilterSelected,
        };

        var cancellationToken = new CancellationTokenSource();
        cancellationToken.CancelAfter(TimeSpan.FromSeconds(5));
        var context = await Mediator.Send(paginationQuery, cancellationToken.Token);
        _serverTable = context.Data;
        _count = context.Count;
        _gamePlayerCount = context.Players;
        _isLoading = false;
        UpdateTitle();
    }

    private void GameLoadData(LoadDataArgs args)
    {
        var gameAliasLookup = new Dictionary<int, string[]>
        {
            [730] = ["cs", "cs2"],
            [240] = ["cs", "css"],
            [10] = ["cs", "cs16"],
            [500] = ["l4d"],
            [550] = ["l4d", "l4d2"],
            [440] = ["tf2"],
            [4000] = ["gmod"]
        };

        var query = _gamesFilterList.AsQueryable();

        if (!string.IsNullOrEmpty(args.Filter))
        {
            var matchingAppIds = gameAliasLookup
                .Where(kvp => kvp.Value.Any(alias => alias.Equals(args.Filter, StringComparison.OrdinalIgnoreCase)))
                .Select(kvp => kvp.Key)
                .ToList();

            query = query.Where(x => x.Name.Contains(args.Filter, StringComparison.OrdinalIgnoreCase)
                                     || x.AppId.ToString().Contains(args.Filter, StringComparison.OrdinalIgnoreCase)
                                     || matchingAppIds.Contains(x.AppId));
        }

        _gamesFilterData = query.ToList();
        InvokeAsync(StateHasChanged);
    }

    private async Task OnDropdownChanged()
    {
        await _dataGrid.GoToPage(0);
        await _dataGrid.Reload();
        UpdateUrl();
    }

    private async Task OnSearch(string text)
    {
        _searchString = text;
        await _dataGrid.GoToPage(0);
        await _dataGrid.Reload();
        UpdateUrl();
    }

    private void UpdateTitle()
    {
        if (_gamesFilterSelected is not null || !string.IsNullOrWhiteSpace(_regionFilterSelected) ||
            !string.IsNullOrWhiteSpace(_searchString))
        {
            _titleText = $"Players {_gamePlayerCount:N0}";
        }
        else
        {
            _titleText = "Servers";
        }

        StateHasChanged();
    }

    private void UpdateUrl()
    {
        var baseUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path);

        var queryString = new Dictionary<string, string?>();

        if (!string.IsNullOrEmpty(_regionFilterSelected))
        {
            queryString["region"] = _regionFilterSelected;
        }

        if (_gamesFilterSelected is not null)
        {
            queryString["game"] = _gamesFilterSelected.AppId.ToString();
        }

        if (!string.IsNullOrEmpty(_searchString))
        {
            queryString["search"] = _searchString;
        }

        var newUri = QueryHelpers.AddQueryString(baseUri, queryString);
        NavigationManager.NavigateTo(newUri, false);
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

            await DialogService.OpenAsync<BlockServerDialog>("Block Address?", parameters, options);
            await OnServerBlockCreated.InvokeAsync();
            await _dataGrid.Reload();
        }
        else
        {
            options.ShowTitle = false;
            await DialogService.OpenAsync<ViewServerMetaDialog>("Server Meta", parameters, options);
            await _dataGrid.Reload();
        }
    }

    private void ShowTooltip(ElementReference elementReference, TooltipOptions? options, string message) =>
        TooltipService.Open(elementReference, message, options);

    public Task ReloadTable() => _dataGrid.Reload();

    private static string PlayerStandardDeviationColour(double? standard)
    {
        if (standard is null) return string.Empty;
        return standard switch
        {
            < 10 => "rz-color-danger-light",
            < 50 => "rz-color-warning-light",
            _ => "rz-color-success-light"
        };
    }

    private static string PlayerGlobalStandardDeviationRatioColour(double? ratio)
    {
        if (ratio is null) return string.Empty;
        return ratio switch
        {
            < 0.5 => "rz-color-danger-light",
            < 1 => "rz-color-warning-light",
            _ => "rz-color-success-light"
        };
    }

    public void Dispose()
    {
        _dataGrid.Dispose();
    }
}
