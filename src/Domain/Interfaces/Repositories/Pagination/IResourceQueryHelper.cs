using BetterSteamBrowser.Domain.ValueObjects.Pagination;

namespace BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;

public interface IResourceQueryHelper<in TQuery, TResult> where TQuery : ValueObjects.Pagination.Pagination where TResult : class
{
    Task<PaginationContext<TResult>> QueryResourceAsync(TQuery request, CancellationToken cancellationToken);
}
