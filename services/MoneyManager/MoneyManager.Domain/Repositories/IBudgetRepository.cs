using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.Repositories;

public interface IBudgetRepository
{
    Task<IEnumerable<Budget>> GetAllAsync();
    Task<Budget?> GetBudgetForUserAndCategoryAsync(Guid userId, Guid categoryId);
    Task<int> AddAsync(Budget budget);
}