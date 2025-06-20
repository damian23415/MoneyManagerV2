using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace UserService.Infrastructure;

public class DbConnectionFactory
{
    private readonly string? _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public virtual async Task<IDbConnection> CreateOpenConnectionAsync()
    {
        if (string.IsNullOrEmpty(_connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        
        var connection = new SqlConnection(_connectionString);
        
        await connection.OpenAsync();
        return connection;
    }
}