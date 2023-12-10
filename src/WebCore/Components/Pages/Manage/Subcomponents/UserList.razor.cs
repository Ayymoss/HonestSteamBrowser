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

public partial class UserList : IDisposable
{
    [Parameter] public string? UserId { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private DialogService DialogService { get; set; }

    private RadzenDataGrid<User> _dataGrid;
    private IEnumerable<User> _userTable;
    private bool _isLoading = true;
    private int _count;
    private string? _searchString;

    private async Task LoadData(LoadDataArgs args)
    {
        _isLoading = true;

        var paginationQuery = new GetUserListCommand
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
        };

        var context = await Mediator.Send(paginationQuery);
        _userTable = context.Data;
        _count = context.Count;
        _isLoading = false;
    }

    private Task OnSearch(string text)
    {
        _searchString = text;
        return _dataGrid.Reload();
    }

    private async Task RowClickEvent(DataGridRowMouseEventArgs<User> arg)
    {
        if (string.IsNullOrWhiteSpace(UserId)) return;
        var parameters = new Dictionary<string, object>
        {
            {"User", arg.Data},
            {"UserId", UserId}
        };

        var options = new DialogOptions
        {
            Style = "min-height:auto;min-width:auto;width:auto;max-width:75%;max-height:97%",
            CloseDialogOnOverlayClick = true
        };

        await DialogService.OpenAsync<ModifyUserDialog>("Modify User?", parameters, options);
        await _dataGrid.Reload();
    }

    public Task ReloadTable() => _dataGrid.Reload();

    public void Dispose()
    {
        _dataGrid.Dispose();
    }
}
