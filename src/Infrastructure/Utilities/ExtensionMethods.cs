using System.Linq.Expressions;
using System.Text.Json;
using System.Text.RegularExpressions;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using Serilog;

namespace BetterSteamBrowser.Infrastructure.Utilities;

public static partial class ExtensionMethods
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public static IQueryable<TDomain> ApplySort<TDomain>(this IQueryable<TDomain> query, SortDescriptor sort,
        Expression<Func<TDomain, object>> property)
    {
        return sort.SortOrder is SortDirection.Ascending
            ? query.OrderBy(property)
            : query.OrderByDescending(property);
    }

    public static async Task<TResponse?> DeserializeHttpResponseContentAsync<TResponse>(this HttpResponseMessage response)
        where TResponse : class
    {
        try
        {
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return string.IsNullOrEmpty(json) ? null : JsonSerializer.Deserialize<TResponse>(json, JsonSerializerOptions);
        }
        catch (Exception e)
        {
            Log.Error("Failed to deserialize response content: {Message}", e.Message);
        }

        return null;
    }
}
