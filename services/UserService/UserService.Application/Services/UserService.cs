using UserService.Application.DTOs;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Repositories;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return null;

        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
        };
    }
}