using UserService.API.GrpcServices;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserGrpcService>();
app.MapGet("/", () => "This is the User gRPC service.");

app.Run();