using MoneyManager.API;
using MoneyManager.Application;
using MoneyManager.Application.EventHandlers;
using MoneyManager.Domain.Events;
using MoneyManager.Infrastructure;
using MoneyManager.Messaging.RabbitMQ.Extensions;
using MoneyManager.Messaging.RabbitMQ.Processing;
using MoneyManager.Messaging.RabbitMQ.Publishing;
using UserService.Proto;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
await builder.Services.AddInfrastructureServices();

builder.Services.AddGrpcClient<UserGrpc.UserGrpcClient>(o =>
{
    o.Address = new Uri("http://localhost:5264"); // <- adres UserService (port z docker-compose lub launchSettings)
});

builder.Services.AddGrpcClient<UserPreferencesService.UserPreferencesServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5264");
});


var budgetCreatedHandler = new BudgetCreatedEventHandler();

var registry = new DomainEventTypeRegistry();
registry.Register<BudgetCreatedEvent>(); // typ z Twojej domeny
var processor = new RabbitMqEventProcessor(registry);

processor.OnEventReceived += async e =>
{
    if (e is BudgetCreatedEvent ev)
    {
        await budgetCreatedHandler.HandleAsync(ev);
    }
};

await processor.StartAsync();
builder.Services.AddScoped<IDomainEventPublisher>(sp => sp.GetRequiredService<RabbitMqDomainEventPublisher>());

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