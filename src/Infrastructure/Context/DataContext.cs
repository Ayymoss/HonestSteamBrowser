using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Domain.Enums;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<EFServer> Servers { get; set; }
    public DbSet<EFBlacklist> Blacklists { get; set; }
    public DbSet<EFSteamGame> SteamGames { get; set; }
    public DbSet<EFFavourite> Favourites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFServer>().ToTable("EFServers");
        modelBuilder.Entity<EFBlacklist>().ToTable("EFBlacklists");
        modelBuilder.Entity<EFSteamGame>().ToTable("EFSteamGames");
        modelBuilder.Entity<EFFavourite>(entity =>
        {
            entity.ToTable("EFFavourites");
            entity.HasKey(e => e.Id);
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired(false); 
        });

        modelBuilder.AddCustomIdentitySeed();
        modelBuilder.AddSteamGamesSeed();
        modelBuilder.AddBlacklistSeed();

        base.OnModelCreating(modelBuilder);
    }
}
