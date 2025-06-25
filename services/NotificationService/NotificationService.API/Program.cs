using NotificationService.Application;
using NotificationService.Domain.GrpcInterfaces;
using NotificationService.Domain.GrpcInterfaces.User;
using NotificationService.Infrastructure.GrpcClient;
using NotificationService.Infrastructure.GrpcClient.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IGrpcClientFactory, GrpcClientFactory>();
builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();

builder.Services.AddEventServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();

