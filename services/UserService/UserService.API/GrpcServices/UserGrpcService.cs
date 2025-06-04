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
        var user = await _userService.GetByIdAsync(Guid.Parse(request.UserId));
        
        if (user == null) 
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        
        return new UserResponse
        {
            UserName = "trelemorele",
            Email = user.Email
        };
    }
}