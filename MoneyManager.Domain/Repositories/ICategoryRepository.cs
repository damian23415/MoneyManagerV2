using MoneyManager.Domain.Entities;

namespace MoneyManager.Domain.Repositories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> GetAllAsync();
}