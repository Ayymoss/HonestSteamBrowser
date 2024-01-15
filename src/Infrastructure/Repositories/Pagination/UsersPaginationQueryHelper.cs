using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Identity;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class UsersPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory)
    : IResourceQueryHelper<GetUserListCommand, User>
{
    public async Task<PaginationContext<User>> QueryResourceAsync(GetUserListCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        var query = GetBaseQuery(context);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = ApplySearchQuery(request, query);

        if (request.Sorts.Any())
            query = ApplySortQuery(request, query);

        var favouriteCounts = await GetFavourites(request, cancellationToken, query, context);

        return await GetPagedData(request, cancellationToken, context, query, favouriteCounts);
    }

    private static async Task<PaginationContext<User>> GetPagedData(GetUserListCommand request, CancellationToken cancellationToken,
        DataContext context, IQueryable<ApplicationUser> query, IReadOnlyDictionary<string, int> favouriteCounts)
    {
        var allUserRoles = await context.UserRoles
            .ToListAsync(cancellationToken: cancellationToken);

        var allRoles = await context.Roles
            .ToListAsync(cancellationToken: cancellationToken);

        var count = await query.CountAsync(cancellationToken: cancellationToken);

        var pagedData = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .ToListAsync(cancellationToken: cancellationToken);

        var result = pagedData.Select(user => new User
        {
            Id = user.Id,
            UserName = user.UserName ?? "< UNSET >",
            RoleName = allUserRoles
                .Where(ur => ur.UserId == user.Id)
                .Join(allRoles,
                    userRole => userRole.RoleId,
                    role => role.Id,
                    (userRole, role) => role.Name)
                .FirstOrDefault() ?? "User",
            FavouriteCount = favouriteCounts.GetValueOrDefault(user.Id, 0),
        }).ToList();

        return new PaginationContext<User>
        {
            Data = result,
            Count = count,
        };
    }

    private static async Task<Dictionary<string, int>> GetFavourites(GetUserListCommand request, CancellationToken cancellationToken,
        IQueryable<ApplicationUser> query,
        DataContext context)
    {
        var userIds = await query
            .Skip(request.Skip)
            .Take(request.Top)
            .Select(u => u.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        var favouriteCounts = await context.Favourites
            .Where(fav => userIds.Contains(fav.UserId))
            .GroupBy(fav => fav.UserId)
            .Select(group => new {UserId = group.Key, Count = group.Count()})
            .ToDictionaryAsync(fav => fav.UserId, fav => fav.Count, cancellationToken);
        return favouriteCounts;
    }

    private static IQueryable<ApplicationUser> ApplySortQuery(GetUserListCommand request, IQueryable<ApplicationUser> query)
    {
        return request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
        {
            "UserName" => current.ApplySort(sort, p => p.UserName ?? string.Empty),
            _ => current
        });
    }

    private static IQueryable<ApplicationUser> ApplySearchQuery(GetUserListCommand request, IQueryable<ApplicationUser> query)
    {
        query = query.Where(search =>
            (search.UserName != null && EF.Functions.ILike(search.UserName, $"%{request.Search}%")));
        return query;
    }

    private static IQueryable<ApplicationUser> GetBaseQuery(DataContext context)
    {
        return context.Users
            .AsNoTracking()
            .AsQueryable();
    }
}
