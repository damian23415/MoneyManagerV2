using UserService.Domain.Entities;

namespace UserService.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<Guid> RegisterUserAsync(User user);
}