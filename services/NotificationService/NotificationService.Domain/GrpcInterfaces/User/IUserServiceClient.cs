using ProtoContracts.User.UserService.Proto;

namespace NotificationService.Domain.GrpcInterfaces.User;

public interface IUserServiceClient
{
    Task<UserResponse?> GetUserByIdAsync(Guid userId);
}