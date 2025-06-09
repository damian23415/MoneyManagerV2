using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.Services;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain.GrpcClients;

namespace MoneyManager.Application;

public static class ApplicationServicesExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<ICategoryService, CategoryService>();

        return services;
    }
}