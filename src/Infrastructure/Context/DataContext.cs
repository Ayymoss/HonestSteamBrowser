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

        var blacklist1 = new EFBlacklist
        {
            Id = -1,
            Value = "188.127.241.66",
            ApiFilter = true,
            Game = SteamGame.CounterStrike2,
            Type = BlacklistType.IpAddress,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist2 = new EFBlacklist
        {
            Id = -2,
            Value = "77.222.37.82",
            ApiFilter = true,
            Game = SteamGame.CounterStrike2,
            Type = BlacklistType.IpAddress,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist3 = new EFBlacklist
        {
            Id = -3,
            Value = "uwujka",
            ApiFilter = true,
            Game = SteamGame.CounterStrike2,
            Type = BlacklistType.GameType,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist4 = new EFBlacklist
        {
            Id = -4,
            Value = "nosteam",
            ApiFilter = true,
            Game = SteamGame.AllGames,
            Type = BlacklistType.GameType,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist5 = new EFBlacklist
        {
            Id = -5,
            Value = "no-steam",
            ApiFilter = true,
            Game = SteamGame.AllGames,
            Type = BlacklistType.GameType,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist6 = new EFBlacklist
        {
            Id = -6,
            Value = "Develop",
            ApiFilter = false,
            Game = SteamGame.CounterStrikeSource,
            Type = BlacklistType.Hostname,
            Added = DateTimeOffset.UtcNow,
        };

        var blacklist7 = new EFBlacklist
        {
            Id = -7,
            Value = "FACEIT",
            ApiFilter = false,
            Game = SteamGame.CounterStrike2,
            Type = BlacklistType.Hostname,
            Added = DateTimeOffset.UtcNow,
        };

        modelBuilder.Entity<EFBlacklist>().HasData(blacklist1);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist2);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist3);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist4);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist5);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist6);
        modelBuilder.Entity<EFBlacklist>().HasData(blacklist7);

        base.OnModelCreating(modelBuilder);
    }
}
