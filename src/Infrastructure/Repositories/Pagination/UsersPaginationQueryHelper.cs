using BetterSteamBrowser.Business.Mediatr.Commands;
using BetterSteamBrowser.Business.ViewModels;
using BetterSteamBrowser.Domain.Interfaces.Repositories.Pagination;
using BetterSteamBrowser.Domain.ValueObjects.Pagination;
using BetterSteamBrowser.Infrastructure.Context;
using BetterSteamBrowser.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Repositories.Pagination;

public class UsersPaginationQueryHelper(IDbContextFactory<DataContext> contextFactory)
    : IResourceQueryHelper<GetUserListCommand, User>
{
    public async Task<PaginationContext<User>> QueryResourceAsync(GetUserListCommand request, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var query = context.Users
            .AsNoTracking()
            .AsQueryable();

        // Filtering and sorting
        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(search =>
                (search.UserName != null && EF.Functions.ILike(search.UserName, $"%{request.Search}%")));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "UserName" => current.ApplySort(sort, p => p.UserName ?? string.Empty),
                _ => current
            });

        // Favourites
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

        // Roles
        var allUserRoles = await context.UserRoles
            .ToListAsync(cancellationToken: cancellationToken);

        var allRoles = await context.Roles
            .ToListAsync(cancellationToken: cancellationToken);

        // Total count
        var count = await query.CountAsync(cancellationToken: cancellationToken);

        // Object mapping
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
}
