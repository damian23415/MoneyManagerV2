using UserService.Proto;

namespace MoneyManager.Domain.GrpcClients;

public interface IUserPreferencesGrpcClient
{
    Task<GetUserPreferencesResponse?> GetUserPreferencesAsync(Guid userId);
}