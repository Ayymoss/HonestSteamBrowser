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

        if (!string.IsNullOrWhiteSpace(request.SearchString))
            query = query.Where(search =>
                (search.UserName != null && EF.Functions.ILike(search.UserName, $"%{request.SearchString}%")));

        if (request.Sorts.Any())
            query = request.Sorts.Aggregate(query, (current, sort) => sort.Property switch
            {
                "UserName" => current.ApplySort(sort, p => p.UserName ?? string.Empty),
                _ => current
            });

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
        }).ToList();

        return new PaginationContext<User>
        {
            Data = result,
            Count = count,
        };
    }
}
