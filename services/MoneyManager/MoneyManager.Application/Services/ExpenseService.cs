using System.Data;
using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Application.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly IUserGrpcClient _userClient;

    public ExpenseService(IExpenseRepository expenseRepository, IBudgetRepository budgetRepository, IDomainEventPublisher eventPublisher, IUserGrpcClient userClient)
    {
        _expenseRepository = expenseRepository;
        _budgetRepository = budgetRepository;
        _eventPublisher = eventPublisher;
        _userClient = userClient;
    }

    public async Task<Guid> AddExpenseAsync(ExpenseDto expenseDto, Guid userId)
    {
        var expense = new Expense(expenseDto.Amount, expenseDto.Date, expenseDto.Description, expenseDto.CategoryId);
        await _expenseRepository.AddAsync(expense);
        
        var budget = await _budgetRepository.GetBudgetForUserAndCategoryAsync(userId, expense.CategoryId);

        if (budget == null)
            return expense.Id;
        
        var totalExpenses = await _expenseRepository.GetTotalExpensesForUserPeriodAsync(userId, budget.StartDate, budget.EndDate);

        if (totalExpenses > budget.Limit)
        {
            var budgetExceededEvent = new BudgetExceededEvent(userId, budget.Id, budget.Category.Name, budget.Limit, totalExpenses);
            await _eventPublisher.PublishAsync(budgetExceededEvent);
        }
        
        await _eventPublisher.PublishAsync(new TransactionCreatedEvent(userId));

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
        };
    }
}