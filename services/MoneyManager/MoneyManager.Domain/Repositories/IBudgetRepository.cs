using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.Repositories;

public interface IBudgetRepository
{
    Task<Budget?> GetBudgetForUserAndCategoryAsync(Guid userId, Guid categoryId);
    Task<int> AddAsync(Budget budget);
}