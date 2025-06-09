using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.Repositories;

public interface IExpenseRepository
{
    Task<Expense?> GetByIdAsync(Guid id);
    Task<int> AddAsync(Expense expense);
    Task<decimal> GetTotalExpensesForUserPeriodAsync(Guid userId, DateTime startDate, DateTime endDate);
}