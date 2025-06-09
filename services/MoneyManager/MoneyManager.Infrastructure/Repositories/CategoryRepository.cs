using Dapper;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.Persistence;

namespace MoneyManager.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public CategoryRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        var sql = "select * from Categories";

        return await connection.QueryAsync<Category>(sql);
    }
}