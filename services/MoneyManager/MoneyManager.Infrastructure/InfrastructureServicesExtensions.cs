using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Domain;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.GrpcClients;
using MoneyManager.Infrastructure.Persistence;
using MoneyManager.Infrastructure.Repositories;

namespace MoneyManager.Infrastructure;

public static class InfrastructureServicesExtensions
{
    public static async Task<IServiceCollection> AddInfrastructureServices(this IServiceCollection services)
    {
        //db connection factory
        services.AddSingleton<DbConnectionFactory>();
        
        //rabbit
        var dispatcher = await RabbitMqDomainEventDispatcher.CreateAsync();
        services.AddSingleton<IDomainEventDispatcher>(dispatcher);
        
        //repos
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IUserGrpcClient, UserGrpcClient>();
        
        return services;
    }
}