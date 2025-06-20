namespace UserService.Application.Services.Interfaces;

public interface IAuthService
{
    Task<string?> LoginAndGetTokenAsync(string email, string password, string secretKey);
}