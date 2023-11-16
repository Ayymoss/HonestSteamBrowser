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
                Id = -1,
                Value = "188.127.241.66",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -2,
                Value = "77.222.37.82",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -3,
                Value = "uwujka",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -4,
                Value = "nosteam",
                ApiFilter = true,
                Game = SteamGame.AllGames,
                Type = BlacklistType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -5,
                Value = "no-steam",
                ApiFilter = true,
                Game = SteamGame.AllGames,
                Type = BlacklistType.GameType,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -6,
                Value = "Develop",
                ApiFilter = false,
                Game = SteamGame.CounterStrikeSource,
                Type = BlacklistType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -7,
                Value = "FACEIT",
                ApiFilter = false,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.Hostname,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -8,
                Value = "185.221.196.80",
                ApiFilter = true,
                Game = SteamGame.Rust,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -9,
                Value = "185.221.196.73",
                ApiFilter = true,
                Game = SteamGame.Rust,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -10,
                Value = "185.221.196.59",
                ApiFilter = true,
                Game = SteamGame.Rust,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -11,
                Value = "185.221.196.58",
                ApiFilter = true,
                Game = SteamGame.Rust,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -12,
                Value = "176.99.12.113",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -13,
                Value = "92.223.109.39",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -14,
                Value = "185.231.246.129",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -15,
                Value = "92.223.109.35",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -16,
                Value = "146.185.218.122",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -17,
                Value = "146.185.215.32",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -18,
                Value = "193.162.143.112",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -19,
                Value = "146.185.218.163",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
            new EFBlacklist
            {
                Id = -20,
                Value = "89.108.83.206",
                ApiFilter = true,
                Game = SteamGame.CounterStrike2,
                Type = BlacklistType.IpAddress,
                Added = DateTimeOffset.UtcNow,
            },
        };

        modelBuilder.Entity<EFBlacklist>().HasData(blacklists);

        base.OnModelCreating(modelBuilder);
    }
}
