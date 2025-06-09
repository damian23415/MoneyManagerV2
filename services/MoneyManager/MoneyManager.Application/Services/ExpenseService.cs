using System.Data;
using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;

namespace MoneyManager.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IDomainEventDispatcher _eventPublisher;
    private readonly IUserGrpcClient _userClient;

    public ExpenseService(IExpenseRepository expenseRepository, IBudgetRepository budgetRepository, IDomainEventDispatcher eventPublisher, IUserGrpcClient userClient)
    {
        _expenseRepository = expenseRepository;
        _budgetRepository = budgetRepository;
        _eventPublisher = eventPublisher;
        _userClient = userClient;
    }

    public async Task<Guid> AddExpenseAsync(ExpenseDto expenseDto)
    {
        var userExists = await _userClient.CheckUserExistsAsync(expenseDto.UserId);
        
        if (!userExists)
            throw new ArgumentException($"User with ID {expenseDto.UserId} does not exist.");
        
        var expense = new Expense(expenseDto.Amount, expenseDto.Date, expenseDto.Description, expenseDto.UserId, expenseDto.CategoryId);
        await _expenseRepository.AddAsync(expense);
        
        var budget = await _budgetRepository.GetBudgetForUserAndCategoryAsync(expense.UserId, expense.CategoryId);

        if (budget == null)
            return expense.Id;
        
        var totalExpenses = await _expenseRepository.GetTotalExpensesForUserPeriodAsync(expense.UserId, budget.StartDate, budget.EndDate);

        if (totalExpenses > budget.Limit)
        {
            var budgetExceededEvent = new BudgetExceededEvent(expense.UserId, budget.Id, budget.Category.Name, budget.Limit, totalExpenses);
            await _eventPublisher.DispatchAsync(budgetExceededEvent);
        }

        return expense.Id;

    }

    public async Task<ExpenseDto?> GetExpenseByIdAsync(Guid id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        if (expense == null)
            return null;

        return new ExpenseDto()
        {
            Id = expense.Id,
            Amount = expense.Amount,
            Date = expense.Date,
            Description = expense.Description,
            CategoryId = expense.CategoryId,
            UserId = expense.UserId,
        };
    }
}