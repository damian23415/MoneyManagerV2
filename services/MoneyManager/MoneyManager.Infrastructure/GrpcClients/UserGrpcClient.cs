using MoneyManager.Domain.GrpcClients;
using UserService.Proto;

namespace MoneyManager.Infrastructure.GrpcClients;

public class UserGrpcClient : IUserGrpcClient
{
    private readonly UserGrpc.UserGrpcClient _client;

    public UserGrpcClient(UserGrpc.UserGrpcClient client)
    {
        _client = client;
    }

    public async Task<bool> CheckUserExistsAsync(Guid userId)
    {
        var response = await _client.GetUserAsync(new UserRequest { UserId = userId.ToString() });
        
        return response != null;
    }
}