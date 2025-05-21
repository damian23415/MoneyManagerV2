using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MoneyManager.Infrastructure.Persistence;

public class MoneyManagerDbContextFactory : IDesignTimeDbContextFactory<MoneyManagerDbContext>
{
    public MoneyManagerDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MoneyManagerDbContext>();
        optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=MoneyManagerDb;Trusted_Connection=true;TrustServerCertificate=true;");

        return new MoneyManagerDbContext(optionsBuilder.Options);
    }
}