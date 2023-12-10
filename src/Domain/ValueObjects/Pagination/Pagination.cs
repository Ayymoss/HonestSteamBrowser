namespace BetterSteamBrowser.Domain.ValueObjects.Pagination;

public class Pagination
{
    public int Top { get; set; }
    public int Skip { get; set; }
    public string? Search { get; set; }
    public IEnumerable<SortDescriptor> Sorts { get; set; }
    public object? Data { get; set; }
}
