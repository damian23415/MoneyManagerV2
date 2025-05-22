using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.Repositories;

namespace MoneyManager.Application.Services;

public class BudgetService(IDomainEventDispatcher dispatcher, IBudgetRepository budgetRepository) : IBudgetService
{
    public Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Budget> CreateBudgetAsync(BudgetDto dto)
    {
        var budget = new Budget(dto.Name, dto.Limit, dto.CategoryId, dto.Month);

        await budgetRepository.AddAsync(budget);
        
        await dispatcher.DispatchAsync(new BudgetCreatedEvent(budget.Id, budget.Name, budget.Limit));

        return budget;
    }
}