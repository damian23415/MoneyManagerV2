using UserService.Application.DTOs;

namespace UserService.Application.Services.Interfaces;

public interface IUserPreferencesService
{
    Task<UserPreferencesDto?> GetUserPreferencesAsync(Guid userId);
}