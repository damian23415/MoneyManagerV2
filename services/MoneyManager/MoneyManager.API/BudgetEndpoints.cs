using System.ComponentModel.DataAnnotations;
using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;

namespace MoneyManager.API;

public static class BudgetEndpoints
{
    public static void MapBudgetEndpoints(this WebApplication app)
    {
        app.MapPost("/budgets/createBudget", async (BudgetDto budgetDto, IBudgetService service) =>
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(budgetDto, null, null);
            
            if (!Validator.TryValidateObject(budgetDto, context, validationResults, true))
                return Results.BadRequest(validationResults);
            
            var createdBudget = await service.CreateBudgetAsync(budgetDto);
            return Results.Created($"/budgets/{createdBudget.Id}", createdBudget);
        })
        .WithName("CreateBudget")
        .WithTags("Budgets");
    }
}