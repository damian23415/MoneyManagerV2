using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.GrpcClients;
using MoneyManager.Infrastructure.Persistence;
using MoneyManager.Infrastructure.Repositories;
using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Infrastructure;

public static class InfrastructureServicesExtensions
{
    public static async Task<IServiceCollection> AddInfrastructureServices(this IServiceCollection services)
    {
        //db connection factory
        services.AddScoped<DbConnectionFactory>();
        
        //rabbitmq
        var publisher = await RabbitMqDomainEventPublisher.CreateAsync();
        services.AddSingleton(publisher);
        
        //repos
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IUserGrpcClient, UserGrpcClient>();
        services.AddScoped<IUserPreferencesGrpcClient, UserPreferencesGrpcClient>();

        
        return services;
    }
}