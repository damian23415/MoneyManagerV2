using UserService.Application.DTOs;
using UserService.Application.DTOs.Request;
using UserService.Application.Enums;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
            return null;

        return new UserDto()
        {
            Id = user.Id,
            Email = user.Email,
            Role = Enum.Parse<UserRole>(user.UserRole),
        };
    }

    public async Task<UserDto?> GetById(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
            return null;
        
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Role = Enum.Parse<UserRole>(user.UserRole),
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<Guid> RegisterUserAsync(UserDto userDto)
    {
        var user = new User(
            userName: userDto.UserName,
            email: userDto.Email,
            passwordHash: _passwordHasher.HashPassword(userDto.Password),
            userRole: userDto.Role.ToString()
        );
        
        return await _userRepository.RegisterUserAsync(user);
    }
}