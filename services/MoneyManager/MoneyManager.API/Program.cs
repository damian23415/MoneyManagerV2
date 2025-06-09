using MoneyManager.API;
using MoneyManager.Application;
using MoneyManager.Infrastructure;
using UserService.Proto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
await builder.Services.AddInfrastructureServices();

builder.Services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
{
    o.Address = new Uri("http://localhost:5264"); // <- adres UserService (port z docker-compose lub launchSettings)
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapCategoryEndpoints();
app.MapBudgetEndpoints();
app.MapExpenseEndpoints();

app.MapGet("/swagger", () => "Hello World!");

app.Run();