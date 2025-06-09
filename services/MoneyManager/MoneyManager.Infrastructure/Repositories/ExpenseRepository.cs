using Dapper;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.Persistence;

namespace MoneyManager.Infrastructure.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly DbConnectionFactory _dbConnectionFactory;

    public ExpenseRepository(DbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        
        var sql = "select * from Expenses where Id = @Id";
        
        return await connection.QuerySingleOrDefaultAsync<Expense>(sql, new { Id = id });
    }

    public async Task<int> AddAsync(Expense expense)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        
        var sql = "insert into Expenses (Id, Amount, Date, Description, UserId, CategoryId) " +
                  "values (@Id, @Amount, @Date, @Description, @UserId, @CategoryId)";
        
        return await connection.ExecuteAsync(sql, expense);
    }

    public async Task<decimal> GetTotalExpensesForUserPeriodAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        using var connection = await _dbConnectionFactory.CreateOpenConnectionAsync();
        
        var sql = "select SUM(Amount) from expense where UserId = @UserId and @Date >= @StartDate and Date <= @EndDate";
        
        return await connection.ExecuteScalarAsync<decimal>(sql, new { UserId = userId, StartDate = startDate, EndDate = endDate });
    }
}