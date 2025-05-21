using Microsoft.EntityFrameworkCore;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.Persistence;

namespace MoneyManager.Infrastructure.Repositories;

public class BudgetRepository : IBudgetRepository
{
    private readonly MoneyManagerDbContext _dbContext;

    public BudgetRepository(MoneyManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Budget>> GetAllAsync()
    {
        return await _dbContext.Budgets.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(Budget budget)
    {
        await _dbContext.Budgets.AddAsync(budget);
    }
}