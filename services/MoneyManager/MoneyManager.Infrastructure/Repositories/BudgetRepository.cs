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

    public async Task<Budget?> GetBudgetForUserAndCategoryAsync(Guid userId, Guid categoryId)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        const string sql = "select Id, Name, [Limit], CategoryId, StartDate, EndDate from Budgets where UserId = @UserId and CategoryId = @CategoryId";
        
        return await connection.QuerySingleOrDefaultAsync<Budget>(sql, new { UserId = userId, CategoryId = categoryId });
    }

    public async Task<int> AddAsync(Budget budget)
    {
        var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();

        const string sql = "insert into Budgets (Id, Name, [Limit], CategoryId, UserId) values (@Id, @Name, @Limit, @CategoryId, @UserId)";
        
        return await connection.ExecuteAsync(sql, new
        {
            budget.Id,
            budget.Name,
            budget.Limit,
            budget.CategoryId,
            budget.UserId
        });
    }
}