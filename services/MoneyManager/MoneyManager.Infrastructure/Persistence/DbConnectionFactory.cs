using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MoneyManager.Infrastructure.Persistence;

public class DbConnectionFactory
{
    private readonly string? _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}