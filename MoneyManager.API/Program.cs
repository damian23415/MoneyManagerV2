using Microsoft.EntityFrameworkCore;
using MoneyManager.API;
using MoneyManager.Application.Services;
using MoneyManager.Application.Services.Interfaces;
using MoneyManager.Domain.Repositories;
using MoneyManager.Infrastructure.Persistence;
using MoneyManager.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();

builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

builder.Services.AddDbContext<MoneyManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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