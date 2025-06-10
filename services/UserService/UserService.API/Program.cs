using UserService.API.GrpcServices;
using UserService.Application.Services;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Repositories;
using UserService.Infrastructure;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserPreferencesService, UserPreferencesService>();
builder.Services.AddScoped<IUserPreferencesRepository, UserPreferencesRepository>();
builder.Services.AddScoped<DbConnectionFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserGrpcService>();
app.MapGrpcService<UserPreferencesGrpc>();
app.MapGet("/", () => "This is the User gRPC service.");

app.Run();