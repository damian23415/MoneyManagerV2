using NotificationService.Domain.GrpcInterfaces;
using NotificationService.Domain.GrpcInterfaces.User;
using ProtoContracts.User.UserService.Proto;

namespace NotificationService.Infrastructure.GrpcClient.User;

public class UserServiceClient : IUserServiceClient
{
    private readonly IGrpcClientFactory _grpcClientFactory;

    public UserServiceClient(IGrpcClientFactory grpcClientFactory)
    {
        _grpcClientFactory = grpcClientFactory;
    }

    public async Task<UserResponse?> GetUserByIdAsync(Guid userId)
    {
        var userClient = _grpcClientFactory.CreateClient<UserService.UserServiceClient>();
        return await userClient.GetUserByIdAsync(new UserRequest { UserId = userId.ToString() });
    }
}