using MoneyManager.Application.Services;
using MoneyManager.Application.Services.Interfaces;

namespace MoneyManager.API;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/categories", async (ICategoryService service) =>
        {
            var categories = await service.GetAllCategoriesAsync();
            return Results.Ok(categories);
        });
    }
}