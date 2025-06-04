using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return new User("john@example.com", "hashed123");
    }
}