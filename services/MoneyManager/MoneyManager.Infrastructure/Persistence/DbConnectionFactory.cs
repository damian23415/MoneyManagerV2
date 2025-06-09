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

    public async Task<IDbConnection> CreateOpenConnectionAsync()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        
        var connection = new SqlConnection(_connectionString);
        
        await connection.OpenAsync();
        return connection;
    }
}