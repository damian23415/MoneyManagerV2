using UserService.Application.DTOs;

namespace UserService.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid userId);
}