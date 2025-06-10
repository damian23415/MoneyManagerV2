using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

public interface IUserPreferencesRepository
{
    Task<UserPreferences?> GetUserPreferencesAsync(Guid userId);
}