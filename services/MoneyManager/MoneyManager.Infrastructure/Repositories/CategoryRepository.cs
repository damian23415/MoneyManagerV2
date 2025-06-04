using System.Data;
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
        var connection = _dbConnectionFactory.CreateConnection();
        var sql = "select * from Categories";

        return await connection.QueryAsync<Category>(sql);
    }
}