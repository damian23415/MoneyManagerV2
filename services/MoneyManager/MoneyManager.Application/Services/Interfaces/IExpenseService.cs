using MoneyManager.Application.DTOs;

namespace MoneyManager.Application.Services.Interfaces;

public interface IExpenseService
{
    Task<Guid> AddExpenseAsync(ExpenseDto expense, Guid userId);
    Task<ExpenseDto?> GetExpenseByIdAsync(Guid id);
}