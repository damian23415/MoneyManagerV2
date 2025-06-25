using UserService.API.Endpoints;
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
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<DbConnectionFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGrpcService<UserGrpcService>();
app.MapGrpcService<UserPreferencesGrpc>();
app.MapUserEndpoints();
app.MapAuthEndpoints();
app.MapGet("/", () => "swagger");

app.Run();