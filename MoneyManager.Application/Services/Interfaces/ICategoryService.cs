using MoneyManager.Application.DTOs;

namespace MoneyManager.Application.Services.Interfaces;

public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetAllCategoriesAsync();
}