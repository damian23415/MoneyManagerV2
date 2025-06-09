using MoneyManager.Application.DTOs;
using MoneyManager.Domain.Entities;

namespace MoneyManager.Application.Services.Interfaces;

public interface IBudgetService
{
    Task<Budget> CreateBudgetAsync(BudgetDto dto);
}