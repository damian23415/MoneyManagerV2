using System.Text;
using Microsoft.IdentityModel.Tokens;
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
    o.Address = new Uri("http://localhost:5002"); // <- adres UserService (port z docker-compose lub launchSettings)
});

builder.Services.AddGrpcClient<UserPreferencesService.UserPreferencesServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5002");
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

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

//await processor.StartAsync(CancellationToken.None);
builder.Services.AddScoped<IDomainEventPublisher>(sp => sp.GetRequiredService<RabbitMqDomainEventPublisher>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Wpisz token JWT w formacie: Bearer {twój_token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
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
app.UseAuthentication();
app.UseAuthorization();

app.Run();