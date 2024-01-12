using fidelappback.Models;
using Microsoft.EntityFrameworkCore;

namespace fidelappback.Database;

// 2. Créer une classe DbContext
public class FidelappDbContext : DbContext
{
    public FidelappDbContext(DbContextOptions<FidelappDbContext> options) : base(options)
    {
    }
    public DbSet<User> User { get; set; }

    // define properties conditions in database that cann't be set with attributes (like unique)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // unique conditions
        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Guid)
            .IsUnique();
    }
}