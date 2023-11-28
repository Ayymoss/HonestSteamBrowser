using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public static class DataContextSeed
{
    public static void AddCustomIdentitySeed(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<MyUser>();

        var adminUser = new MyUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "Admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "adminadmin"),
            SecurityStamp = string.Empty
        };

        var adminRole = new IdentityRole
        {
            Id = Guid.NewGuid().ToString(),
            Name = "Admin",
            NormalizedName = "ADMIN"
        };

        modelBuilder.Entity<MyUser>().HasData(adminUser);
        modelBuilder.Entity<IdentityRole>().HasData(adminRole);
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = adminRole.Id,
            UserId = adminUser.Id
        });
    }

    public static void AddBlacklistSeed(this ModelBuilder modelBuilder)
    {
        var blacklists = new[]
        {
            new EFBlacklist
            {
                Id = -3,
                Value = "uwujka",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -4,
                Value = "nosteam",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -5,
                Value = "no-steam",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -6,
                Value = "Develop",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrikeSource,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -7,
                Value = "FACEIT",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -8,
                Value = "RU",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -9,
                Value = "JP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -10,
                Value = "CN",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -11,
                Value = "RU",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -12,
                Value = "JP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -13,
                Value = "CN",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -14,
                Value = "Develop",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -15,
                Value = "FASTCUP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
        };

        modelBuilder.Entity<EFBlacklist>().HasData(blacklists);
    }

    public static void AddSteamGamesSeed(this ModelBuilder modelBuilder)
    {
        var gameList = new[]
        {
            new EFSteamGame {Id = -2, AppId = -2, Name = "All Games"},
            new EFSteamGame {Id = -1, AppId = -1, Name = "Unknown"},
            new EFSteamGame {Id = 1, AppId = 10, Name = "Counter-Strike"},
            new EFSteamGame {Id = 2, AppId = 240, Name = "Counter-Strike: Source"},
            new EFSteamGame {Id = 3, AppId = 730, Name = "Counter-Strike 2"},
            new EFSteamGame {Id = 4, AppId = 252490, Name = "Rust"},
        };

        modelBuilder.Entity<EFSteamGame>().HasData(gameList);
    }
}
