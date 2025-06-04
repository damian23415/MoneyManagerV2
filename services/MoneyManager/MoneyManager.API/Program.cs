using System.Data;
using Microsoft.Data.SqlClient;
using MoneyManager.API;
using MoneyManager.Application.Services;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure;
using MoneyManager.Infrastructure.GrpcClients;
using MoneyManager.Infrastructure.Persistence;
using MoneyManager.Infrastructure.Repositories;
using UserService.Proto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<IUserGrpcClient, UserGrpcClient>();

var dispatcher = await RabbitMqDomainEventDispatcher.CreateAsync();
builder.Services.AddSingleton<IDomainEventDispatcher>(dispatcher);
builder.Services.AddSingleton<DbConnectionFactory>();

builder.Services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
{
    o.Address = new Uri("http://localhost:5264"); // <- adres UserService (port z docker-compose lub launchSettings)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCategoryEndpoints();
app.MapBudgetEndpoints();

app.MapGet("/swagger", () => "Hello World!");

app.Run();