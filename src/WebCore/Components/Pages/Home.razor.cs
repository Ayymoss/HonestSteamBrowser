using BetterSteamBrowser.Business.DTOs;
using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Domain.Enums;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SortDescriptor = BetterSteamBrowser.Domain.ValueObjects.Pagination.SortDescriptor;

namespace BetterSteamBrowser.WebCore.Components.Pages;

public partial class Home
{
    [Inject] private IMediator Mediator { get; set; }
    private RadzenDataGrid<Server> _dataGrid;
    private IEnumerable<Server> _serverTable;
    IEnumerable<string> _dropdownGames;
    private string? _dropdownSelected;
    private bool _isLoading = true;
    private int _count;
    private int _playerCount;
    private string? _searchString;

    protected override Task OnInitializedAsync()
    {
        _dropdownGames = Enum.GetValues(typeof(SteamGame))
            .Cast<SteamGame>()
            .Where(game => game is not SteamGame.Unknown)
            .Where(game => game is not SteamGame.AllGames)
            .Select(game => game.Humanize().Transform(To.TitleCase))
            .Order()
            .ToList();

        return base.OnInitializedAsync();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;
        SteamGame? selectedGame;

        try
        {
            selectedGame = _dropdownSelected.DehumanizeTo<SteamGame>();
        }
        catch
        {
            selectedGame = null;
        }

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
            Data = selectedGame,
            Top = args.Top ?? 20,
            Skip = args.Skip ?? 0
        };

        var context = await Mediator.Send(paginationQuery);
        _serverTable = context.Data;
        _count = context.Count;
        _playerCount = context.ExtraData is int data ? data : 0;
        _isLoading = false;
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _dataGrid.Reload();
    }

    private void OnDropdownChanged()
    {
        _dataGrid.Reload();
    }
}
