using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Infrastructure.Persistence;

public class MoneyManagerDbContext(DbContextOptions<MoneyManagerDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>().HasData(
            new 
            {
                Id = Guid.Parse("a1111111-1111-1111-1111-111111111111"),
                Name = "Jedzenie"
            },
            new 
            {
                Id = Guid.Parse("b2222222-2222-2222-2222-222222222222"),
                Name = "Transport"
            }
        );
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}