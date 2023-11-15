using System.Linq.Expressions;
using System.Text.RegularExpressions;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;

namespace BetterSteamBrowser.Infrastructure.Utilities;

public static partial class ExtensionMethods
{
    public static IQueryable<TDomain> ApplySort<TDomain>(this IQueryable<TDomain> query, SortDescriptor sort,
        Expression<Func<TDomain, object>> property)
    {
        return sort.SortOrder is SortDirection.Ascending
            ? query.OrderBy(property)
            : query.OrderByDescending(property);
    }
    public static string FilterUnknownCharacters(this string input)
    {
        var regex = MyRegex();
        var cleanedName = regex.Replace(input, "");
        return string.IsNullOrEmpty(cleanedName) ? "Unknown" : cleanedName;
    }

    [GeneratedRegex(@"[^\p{L}\p{P}\p{N}]")]
    private static partial Regex MyRegex();
}
