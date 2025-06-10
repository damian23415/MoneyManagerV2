using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Repositories;

namespace UserService.Application.Services;

public class UserPreferencesService : IUserPreferencesService
{
    private readonly IUserPreferencesRepository _userPreferencesRepository;

    public UserPreferencesService(IUserPreferencesRepository userPreferencesRepository)
    {
        _userPreferencesRepository = userPreferencesRepository;
    }

    public async Task<UserPreferencesDto?> GetUserPreferencesAsync(Guid userId)
    {
        var userPreferences = await _userPreferencesRepository.GetUserPreferencesAsync(userId);

        return userPreferences == null 
            ? null 
            : new UserPreferencesDto(userPreferences.UserId, userPreferences.PreferredCurrency, userPreferences.Language, userPreferences.EmailNotificationsEnabled, userPreferences.CreatedAt, userPreferences.UpdatedAt);
    }
}