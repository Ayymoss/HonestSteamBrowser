using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<EFServer> Servers { get; set; }
    public DbSet<EFBlacklist> Blacklists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFServer>().ToTable("EFServers");
        modelBuilder.Entity<EFBlacklist>().ToTable("EFBlacklists");

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
