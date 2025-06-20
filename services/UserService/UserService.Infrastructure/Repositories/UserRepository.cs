using Dapper;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public UserRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<User?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        const string sql = "select Id, Email, PasswordHash, UserRole from Users where Id = @Id";
        
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<Guid> RegisterUserAsync(User user)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();

        const string sql =
            "insert into Users (Id, UserName, Email, PasswordHash, UserRole, CreatedAt) values (@Id, @UserName, @Email, @PasswordHash, @UserRole, @CreatedAt)";

        await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.Email,
            user.PasswordHash,
            user.UserRole,
            CreatedAt = DateTimeOffset.Now
        });

        return user.Id;
    }
}