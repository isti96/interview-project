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
}