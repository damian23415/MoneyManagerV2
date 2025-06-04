using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.Repositories;

public interface IBudgetRepository
{
    Task<IEnumerable<Budget>> GetAllAsync();
    Task AddAsync(Budget budget);
}