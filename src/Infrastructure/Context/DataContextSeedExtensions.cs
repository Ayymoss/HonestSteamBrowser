using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Domain.Interfaces;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IdentityRole = Microsoft.AspNetCore.Identity.IdentityRole;

namespace BetterSteamBrowser.Infrastructure.Context;

public static class DataContextSeed
{
    public static void AddCustomIdentitySeed(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<ApplicationUser>();

        var adminUser = new ApplicationUser
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

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
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
                Id = -1,
                Value = "FASTCUP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -2,
                Value = "uwujka",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -3,
                Value = "nosteam",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -4,
                Value = "no-steam",
                ApiFilter = true,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -5,
                Value = "Develop",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -6,
                Value = "FACEIT",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.AllGames,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -7,
                Value = "RU",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -8,
                Value = "JP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -9,
                Value = "CN",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -10,
                Value = "RU",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -11,
                Value = "JP",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -12,
                Value = "CN",
                ApiFilter = false,
                SteamGameId = SteamGameConstants.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            }
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
            new EFSteamGame {Id = 2, AppId = 30, Name = "Day of Defeat"},
            new EFSteamGame {Id = 3, AppId = 50, Name = "Half-Life: Opposing Force"},
            new EFSteamGame {Id = 4, AppId = 70, Name = "Half-Life"},
            new EFSteamGame {Id = 5, AppId = 80, Name = "Condition Zero"},
            new EFSteamGame {Id = 6, AppId = 240, Name = "Counter-Strike: Source"},
            new EFSteamGame {Id = 7, AppId = 300, Name = "Day of Defeat: Source"},
            new EFSteamGame {Id = 8, AppId = 320, Name = "Half-Life 2: Deathmatch"},
            new EFSteamGame {Id = 9, AppId = 440, Name = "Team Fortress 2"},
            new EFSteamGame {Id = 10, AppId = 500, Name = "Left 4 Dead"},
            new EFSteamGame {Id = 11, AppId = 550, Name = "Left 4 Dead 2"},
            new EFSteamGame {Id = 12, AppId = 730, Name = "Counter-Strike 2"},
            new EFSteamGame {Id = 13, AppId = 1250, Name = "Killing Floor"},
            new EFSteamGame {Id = 14, AppId = 4000, Name = "Garry's Mod"},
            new EFSteamGame {Id = 15, AppId = 4920, Name = "Natural Selection 2"},
            new EFSteamGame {Id = 16, AppId = 17520, Name = "Synergy"},
            new EFSteamGame {Id = 17, AppId = 17550, Name = "Eternal Silence"},
            new EFSteamGame {Id = 18, AppId = 33930, Name = "Arma 2: Operation Arrowhead"},
            new EFSteamGame {Id = 19, AppId = 107410, Name = "Arma 3"},
            new EFSteamGame {Id = 20, AppId = 108600, Name = "Project Zomboid"},
            new EFSteamGame {Id = 21, AppId = 221100, Name = "DayZ"},
            new EFSteamGame {Id = 22, AppId = 222880, Name = "Insurgency"},
            new EFSteamGame {Id = 23, AppId = 232090, Name = "Killing Floor 2"},
            new EFSteamGame {Id = 24, AppId = 242760, Name = "The Forest"},
            new EFSteamGame {Id = 25, AppId = 246900, Name = "Viscera Cleanup Detail"},
            new EFSteamGame {Id = 26, AppId = 251570, Name = "7 Days to Die"},
            new EFSteamGame {Id = 27, AppId = 252490, Name = "Rust"},
            new EFSteamGame {Id = 28, AppId = 304930, Name = "Unturned"},
            new EFSteamGame {Id = 29, AppId = 311210, Name = "Call of Duty: Black Ops III"},
            new EFSteamGame {Id = 30, AppId = 312660, Name = "Sniper Elite 4"},
            new EFSteamGame {Id = 31, AppId = 393380, Name = "Squad"},
            new EFSteamGame {Id = 32, AppId = 394690, Name = "Tower Unite"},
            new EFSteamGame {Id = 33, AppId = 466560, Name = "Northgard"},
            new EFSteamGame {Id = 34, AppId = 632360, Name = "Risk of Rain 2"},
            new EFSteamGame {Id = 35, AppId = 686810, Name = "Hell Let Loose"},
            new EFSteamGame {Id = 36, AppId = 1604030, Name = "V Rising"},
            new EFSteamGame {Id = 37, AppId = 346110, Name = "ARK: Survival Evolved"},
        };

        modelBuilder.Entity<EFSteamGame>().HasData(gameList);
    }
}
