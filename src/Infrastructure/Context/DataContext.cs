using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MyUser>(options)
{
    public DbSet<EFServer> Servers { get; set; }
    public DbSet<EFBlacklist> Blacklists { get; set; }
    public DbSet<EFSteamGame> SteamGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFServer>().ToTable("EFServers");
        modelBuilder.Entity<EFBlacklist>().ToTable("EFBlacklists");
        modelBuilder.Entity<EFSteamGame>().ToTable("EFSteamGames");

        modelBuilder.AddCustomIdentitySeed();
        modelBuilder.AddSteamGamesSeed();
        modelBuilder.AddBlacklistSeed();

        base.OnModelCreating(modelBuilder);
    }
}
