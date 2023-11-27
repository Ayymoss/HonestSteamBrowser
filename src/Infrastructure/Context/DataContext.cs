using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MyUser>(options)
{
    public DbSet<EFServer> Servers { get; set; }
    public DbSet<EFBlacklist> Blacklists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFServer>().ToTable("EFServers");
        modelBuilder.Entity<EFBlacklist>().ToTable("EFBlacklists");

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

        var blacklists = new[]
        {
            new EFBlacklist
            {
                Id = -3,
                Value = "uwujka",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -4,
                Value = "nosteam",
                ApiFilter = true,
                Game = SteamGame.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -5,
                Value = "no-steam",
                ApiFilter = true,
                Game = SteamGame.AllGames,
                Type = FilterType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -6,
                Value = "Develop",
                ApiFilter = false,
                Game = SteamGame.CounterStrikeSource,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -7,
                Value = "FACEIT",
                ApiFilter = false,
                Game = SteamGame.CounterStrike2,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -8,
                Value = "RU",
                ApiFilter = false,
                Game = SteamGame.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -9,
                Value = "JP",
                ApiFilter = false,
                Game = SteamGame.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -10,
                Value = "CN",
                ApiFilter = false,
                Game = SteamGame.CounterStrike2,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -11,
                Value = "RU",
                ApiFilter = false,
                Game = SteamGame.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -12,
                Value = "JP",
                ApiFilter = false,
                Game = SteamGame.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -13,
                Value = "CN",
                ApiFilter = false,
                Game = SteamGame.Rust,
                Type = FilterType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -14,
                Value = "Develop",
                ApiFilter = false,
                Game = SteamGame.CounterStrike,
                Type = FilterType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
        };

        modelBuilder.Entity<EFBlacklist>().HasData(blacklists);

        base.OnModelCreating(modelBuilder);
    }
}
