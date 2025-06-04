using System.Data;
using Dapper;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.Persistence;

namespace MoneyManager.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;
    
    public BudgetRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<IEnumerable<Budget>> GetAllAsync()
    {
        const string sql = "select Id, Name, Limit, Category from Budgets";
        var connection = _dbConnectionFactory.CreateConnection();
        
        return await connection.QueryAsync<Budget>(sql);
    }

    public async Task AddAsync(Budget budget)
    {
        var connection = _dbConnectionFactory.CreateConnection();

        const string sql = "insert into Budgets (Id, Name, Limit, Category) values (@Id, @Name, @Limit, @Category)";
        
        await connection.ExecuteAsync(sql, new
        {
            budget.Id,
            budget.Name,
            budget.Limit,
            Category = budget.CategoryId
        });
    }
}