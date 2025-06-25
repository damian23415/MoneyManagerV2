using Events.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoneyManager.Messaging.RabbitMQ.Extensions;
using MoneyManager.Messaging.RabbitMQ.Processing;
using NotificationService.Application.EventsProcessor;

namespace NotificationService.Application;

public static class EventServiceExtension
{
    public static void AddEventServices(this IServiceCollection services)
    {
        services.AddScoped<TransactionCreatedHandler>();

        var registry = new DomainEventTypeRegistry();
        registry.Register<TransactionCreatedEvent>();
        services.AddSingleton(registry);
        
        services.AddSingleton<RabbitMqEventProcessor>();

        services.AddSingleton<IHostedService>(sp =>
        {
            var processor = sp.GetRequiredService<RabbitMqEventProcessor>();
            
            processor.OnEventReceived += async e =>
            {
                if (e is TransactionCreatedEvent ev)
                {
                    using var scope = sp.CreateScope();
                    var handler = scope.ServiceProvider.GetRequiredService<TransactionCreatedHandler>();
                    await handler.HandleAsync(ev);
                }
            };
            processor.StartAsync(CancellationToken.None).GetAwaiter().GetResult();

            return processor;
        });
    }
}