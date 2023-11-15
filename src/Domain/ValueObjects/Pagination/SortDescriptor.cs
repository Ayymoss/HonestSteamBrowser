using BetterSteamBrowser.Domain.Enums;

namespace BetterSteamBrowser.Domain.ValueObjects.Pagination;

public class SortDescriptor
{
    public string Property { get; set; }
    public SortDirection SortOrder { get; set; }
}
