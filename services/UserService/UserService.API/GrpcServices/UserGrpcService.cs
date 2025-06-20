using Grpc.Core;
using UserService.Application.Services.Interfaces;
using UserService.Proto;

namespace UserService.API.GrpcServices;

public class UserGrpcService : UserGrpc.UserGrpcBase
{
    private readonly IUserService _userService;

    public UserGrpcService(IUserService userService)
    {
        _userService = userService;
    }

    public override async Task<UserResponse> GetUser(UserRequest request, ServerCallContext context)
    {
        var user = await _userService.GetByEmail(request.Email);
        
        if (user == null) 
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        
        return new UserResponse
        {
            UserName = user.UserName,
            Email = user.Email
        };
    }
}