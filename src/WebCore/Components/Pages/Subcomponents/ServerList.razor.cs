using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Domain.Enums;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Radzen;
using Radzen.Blazor;
using SortDescriptor = BetterSteamBrowser.Domain.ValueObjects.Pagination.SortDescriptor;

namespace BetterSteamBrowser.WebCore.Components.Pages.Subcomponents;

public partial class ServerList : IDisposable
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }

    private RadzenDataGrid<Server> _dataGrid;
    private IEnumerable<Server> _serverTable;
    private IEnumerable<string> _dropdownGames;
    private string? _dropdownSelected;
    private SteamGame? _dropdownSelectedGame;
    private bool _isLoading = true;
    private int _count;
    private int _gamePlayerCount;
    private string? _searchString;
    private string _titleText = "Servers";

    protected override Task OnInitializedAsync()
    {
        _dropdownGames = Enum.GetValues(typeof(SteamGame))
            .Cast<SteamGame>()
            .Where(x => x is not SteamGame.Unknown)
            .Where(x => x is not SteamGame.AllGames)
            .Select(x => x.Humanize().Transform(To.TitleCase))
            .Order()
            .ToList();

        var uri = new Uri(NavigationManager.Uri);
        var query = QueryHelpers.ParseQuery(uri.Query);

        query.TryGetValue("game", out var game);
        query.TryGetValue("filter", out var filter);
        _dropdownSelectedGame = Enum.TryParse<SteamGame>(game.FirstOrDefault(), out var steamGame) ? steamGame : null;
        _dropdownSelected = _dropdownSelectedGame?.Humanize().Transform(To.TitleCase);
        _searchString = filter.FirstOrDefault();
        return base.OnInitializedAsync();
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
            Data = _dropdownSelectedGame,
            Top = args.Top ?? 20,
            Skip = args.Skip ?? 0
        };

        var context = await Mediator.Send(paginationQuery);
        _serverTable = context.Data;
        _count = context.Count;
        _gamePlayerCount = context.Players;
        _isLoading = false;
        UpdateTitle();
    }

    private void OnDropdownChanged()
    {
        try
        {
            _dropdownSelectedGame = _dropdownSelected.DehumanizeTo<SteamGame>();
        }
        catch
        {
            _dropdownSelectedGame = null;
        }

        _dataGrid.Reload();
        UpdateUrl();
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _dataGrid.Reload();
        UpdateUrl();
    }

    private void UpdateTitle()
    {
        _titleText = _dropdownSelectedGame is not null ? $"Players {_gamePlayerCount:N0}" : "Servers";
        StateHasChanged();
    }

    private void UpdateUrl()
    {
        var baseUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path);

        var queryString = new Dictionary<string, string?>();
        if (_dropdownSelectedGame is not null)
        {
            queryString["game"] = ((int)_dropdownSelectedGame).ToString();
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
}
