using interview_project.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace interview_project.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Map tables to lowercase names
        modelBuilder.Entity<AppUser>().ToTable("appusers");
        modelBuilder.Entity<Company>().ToTable("companies");

        // Optional: map columns to lowercase too
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserName).HasColumnName("username");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.StockTicker).HasColumnName("stockticker");
            entity.Property(e => e.Exchange).HasColumnName("exchange");
            entity.Property(e => e.ISIN).HasColumnName("isin");
            entity.Property(e => e.WebsiteURL).HasColumnName("websiteurl");
        });
    }
}