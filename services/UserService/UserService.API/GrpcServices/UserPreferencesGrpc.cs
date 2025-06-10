
using Grpc.Core;
using UserService.Application.Services.Interfaces;
using UserService.Proto;

namespace UserService.API.GrpcServices;

public class UserPreferencesGrpc : UserPreferencesService.UserPreferencesServiceBase
{
    private readonly IUserPreferencesService _preferencesService;

    public UserPreferencesGrpc(IUserPreferencesService preferencesService)
    {
        _preferencesService = preferencesService;
    }

    public override async Task<GetUserPreferencesResponse> GetUserPreferences(GetUserPreferencesRequest request,
        ServerCallContext context)
    {
        if (request == null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Request cannot be null"));
        
        if (string.IsNullOrEmpty(request.UserId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "UserId cannot be null or empty"));
        
        if (!Guid.TryParse(request.UserId, out var userId))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid UserId format"));
        
        
        var preferences = await _preferencesService.GetUserPreferencesAsync(userId);
        
        if (preferences == null)
            throw new RpcException(new Status(StatusCode.NotFound, "User preferences not found"));
        
        return new GetUserPreferencesResponse
        {
            UserId = preferences.UserId.ToString(),
            PreferredCurrency = preferences.PreferredCurrency,
            Language = preferences.Language,
            EmailNotificationsEnabled = preferences.EmailNotificationsEnabled,
        };
    }
}