using System.ComponentModel.DataAnnotations;
using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services.Interfaces;

namespace MoneyManager.API;

public static class ExpenseEndpoints
{
    public static void MapExpenseEndpoints(this WebApplication app)
    {
        app.MapPost("/expenses/createExpense", async (ExpenseDto expenseDto, IExpenseService service) =>
            {
                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(expenseDto, null, null);
            
                if (!Validator.TryValidateObject(expenseDto, context, validationResults, true))
                    return Results.BadRequest(validationResults);
            
                var createdExpenseId = await service.AddExpenseAsync(expenseDto);
                return Results.Created($"/expenses/{createdExpenseId}", createdExpenseId);
            })
            .WithName("CreateExpense")
            .WithTags("Expenses");
        
        
        app.MapGet("/expenses/{id:guid}", async (Guid expenseId, IExpenseService service) =>
            {
               var expense = await service.GetExpenseByIdAsync(expenseId);
               
               return expense == null ? Results.NotFound() : Results.Ok(expense);
            })
            .WithName("GetExpenseById")
            .WithTags("Expenses");
    }
}