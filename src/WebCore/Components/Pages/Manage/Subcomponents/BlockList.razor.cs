using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.WebCore.Components.Pages.Manage.Dialogs;
using MediatR;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using SortDescriptor = BetterSteamBrowser.Domain.ValueObjects.Pagination.SortDescriptor;

namespace BetterSteamBrowser.WebCore.Components.Pages.Manage.Subcomponents;

public partial class BlockList : IDisposable
{
    [Parameter] public string? UserId { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private DialogService DialogService { get; set; }

    private RadzenDataGrid<Block> _dataGrid;
    private IEnumerable<Block> _blockTable;
    private IEnumerable<SteamGame> _dropDownGames;
    private SteamGame? _dropDownSelected;
    private bool _isLoading = true;
    private int _count;
    private string? _searchString;

    protected override async Task OnInitializedAsync()
    {
        var steamGames = await Mediator.Send(new GetSteamGamesCommand());
        _dropDownGames = steamGames.OrderBy(x => x.Name);
        await base.OnInitializedAsync();
    }

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;

        var paginationQuery = new GetBlockListCommand
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
        };

        var context = await Mediator.Send(paginationQuery);
        _blockTable = context.Data;
        _count = context.Count;
        _isLoading = false;
    }

    private Task OnDropdownChanged() => _dataGrid.Reload();

    private Task OnSearch(string text)
    {
        _searchString = text;
        return _dataGrid.Reload();
    }

    private async Task RowClickEvent(DataGridRowMouseEventArgs<Block> arg)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        var parameters = new Dictionary<string, object>
        {
            {"Block", arg.Data},
            {"UserId", UserId}
        };

        var options = new DialogOptions
        {
            Style = "min-height:auto;min-width:auto;width:auto;max-width:75%;max-height:97%",
            CloseDialogOnOverlayClick = true
        };

        await DialogService.OpenAsync<RemoveBlockDialog>("Remove Block?", parameters, options);
        await _dataGrid.Reload();
    }
    
    public Task ReloadTable() => _dataGrid.Reload();

    public void Dispose()
    {
        _dataGrid.Dispose();
    }
}
