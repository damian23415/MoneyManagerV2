using System.ComponentModel.DataAnnotations;
using UserService.Application.DTOs.Request;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginRequest loginRequest, IAuthService authService, IConfiguration configuration) =>
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(loginRequest, null, null);
            
            if (!Validator.TryValidateObject(loginRequest, context, validationResults, true))
                return Results.BadRequest(validationResults);

            var token = await authService.LoginAndGetTokenAsync(loginRequest.Email, loginRequest.Password, configuration["Jwt:Key"]);

            if (string.IsNullOrEmpty(token))
                return Results.Unauthorized();

            return Results.Ok(new { Token = token });
        })
        .WithName("Login")
        .WithTags("Authentication");
    }
}