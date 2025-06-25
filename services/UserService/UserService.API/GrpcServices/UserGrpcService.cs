using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using ProtoContracts.User.UserService.Proto;
using UserService.Application.Services.Interfaces;

namespace UserService.API.GrpcServices;

public class UserGrpcService : ProtoContracts.User.UserService.Proto.UserService.UserServiceBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<UserResponse> GetUserById(UserRequest request, ServerCallContext context)
    {
        var user = await _userService.GetById(Guid.Parse(request.UserId));
        
        if (user == null) 
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        
        return new UserResponse
        {
            UserName = user.UserName,
            Email = user.Email,
            UserRole = (UserRole)user.Role,
            CreatedAt = Timestamp.FromDateTime(user.CreatedAt.ToUniversalTime())
        };
    }
}