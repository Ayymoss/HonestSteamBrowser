using BetterSteamBrowser.Domain.Entities;
using BetterSteamBrowser.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BetterSteamBrowser.Infrastructure.Context;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<EFServer> Servers { get; set; }
    public DbSet<EFBlock> Blocks { get; set; }
    public DbSet<EFSteamGame> SteamGames { get; set; }
    public DbSet<EFFavourite> Favourites { get; set; }
    public DbSet<EFSnapshot> Snapshots { get; set; }
    public DbSet<EFServerSnapshot> ServerSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EFServer>(entity =>
        {
            entity.ToTable("EFServers");
            entity.HasIndex(e => e.IpAddress);
            entity.HasIndex(e => e.Map);
            entity.HasIndex(e => e.Name);
        });
        modelBuilder.Entity<EFSnapshot>().ToTable("EFSnapshots");
        modelBuilder.Entity<EFServerSnapshot>().ToTable("EFServerSnapshots");
        modelBuilder.Entity<EFBlock>().ToTable("EFBlocks");
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
        modelBuilder.AddBlocksSeed();

        base.OnModelCreating(modelBuilder);
    }
}
