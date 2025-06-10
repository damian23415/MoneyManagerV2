using System.Data;
using Dapper;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserPreferencesRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<UserPreferences?> GetUserPreferencesAsync(Guid userId)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        const string sql = "SELECT * FROM UserPreferences WHERE UserId = @UserId";
        
        return await connection.QuerySingleOrDefaultAsync<UserPreferences>(sql, new { UserId = userId });
    }
}