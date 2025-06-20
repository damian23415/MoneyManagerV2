using System.ComponentModel.DataAnnotations;
using UserService.Application.DTOs;
using UserService.Application.DTOs.Request;
using UserService.Application.Services.Interfaces;

namespace UserService.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users/register", async (UserDto userDto, IUserService userService) =>
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(userDto, null, null);

            if (!Validator.TryValidateObject(userDto, context, validationResults, true))
                return Results.BadRequest(validationResults);

            var createdUserId = await userService.RegisterUserAsync(userDto);
            return Results.Created($"/users/{createdUserId}", createdUserId);
        })
        .WithName("RegisterUser")
        .WithTags("Users");
    }
}