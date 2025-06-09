using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;

namespace MoneyManager.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUserGrpcClient _userClient;

    public BudgetService(IDomainEventDispatcher dispatcher, IBudgetRepository budgetRepository, IUserGrpcClient userClient)
    {
        _dispatcher = dispatcher;
        _budgetRepository = budgetRepository;
        _userClient = userClient;
    }
    

    public async Task<Budget> CreateBudgetAsync(BudgetDto dto)
    {
        var userExists = await _userClient.CheckUserExistsAsync(dto.UserId);
        
        if (!userExists)
            throw new ArgumentException($"User with ID {dto.UserId} does not exist.");
        
        var budget = new Budget(dto.Name, dto.Limit, dto.CategoryId, dto.UserId, dto.StartDate, dto.EndDate);

        await _budgetRepository.AddAsync(budget);
        
        await _dispatcher.DispatchAsync(new BudgetCreatedEvent(budget.Id, budget.Name, budget.Limit));

        return budget;
    }
}