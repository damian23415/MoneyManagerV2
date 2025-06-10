using MoneyManager.Domain.GrpcClients;
using UserService.Proto;

namespace MoneyManager.Infrastructure.GrpcClients;

public class UserPreferencesGrpcClient : IUserPreferencesGrpcClient
{
    private readonly UserPreferencesService.UserPreferencesServiceClient _client;

    public UserPreferencesGrpcClient(UserPreferencesService.UserPreferencesServiceClient client)
    {
        _client = client;
    }

    public async Task<GetUserPreferencesResponse?> GetUserPreferencesAsync(Guid userId)
    {
        return await _client.GetUserPreferencesAsync(new GetUserPreferencesRequest { UserId = userId.ToString() });
    }
}