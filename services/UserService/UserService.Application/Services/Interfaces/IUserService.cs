using UserService.Application.DTOs;
using UserService.Application.DTOs.Request;

namespace UserService.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByEmail(string email);
    Task<Guid> RegisterUserAsync(UserDto userDto);
}