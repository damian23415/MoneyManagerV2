﻿using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IDomainEventPublisher _publisher;
    private readonly IBudgetRepository _budgetRepository;
    private readonly IUserGrpcClient _userClient;
    private readonly IUserPreferencesGrpcClient _userPreferencesClient;

    public BudgetService(IDomainEventPublisher publisher, IBudgetRepository budgetRepository, IUserGrpcClient userClient, IUserPreferencesGrpcClient userPreferencesClient)
    {
        _publisher = publisher;
        _budgetRepository = budgetRepository;
        _userClient = userClient;
        _userPreferencesClient = userPreferencesClient;
    }
    

    public async Task<Budget> CreateBudgetAsync(BudgetDto dto)
    {
        var userExists = await _userClient.CheckUserExistsAsync(dto.UserId);
        
        if (!userExists)
            throw new ArgumentException($"User with ID {dto.UserId} does not exist.");
        
        var userPreferences = await _userPreferencesClient.GetUserPreferencesAsync(dto.UserId);

        if (userPreferences != null && userPreferences.PreferredCurrency != "PLN")
        {
            // obsługa innych wallut, dojdzie w przyszłości, na razie ma to na celu naukę korzystania z gRPC
        }
        
        var budget = new Budget(dto.Name, dto.Limit, dto.CategoryId, dto.UserId, dto.StartDate, dto.EndDate);

        await _budgetRepository.AddAsync(budget);
        
        await _publisher.PublishAsync(new BudgetCreatedEvent(budget.Id, budget.Name, budget.Limit));

        return budget;
    }
}