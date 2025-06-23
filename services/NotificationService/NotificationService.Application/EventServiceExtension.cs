using Events.Events;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Messaging.RabbitMQ.Extensions;
using MoneyManager.Messaging.RabbitMQ.Processing;
using NotificationService.Application.EventsProcessor;

namespace NotificationService.Application;

public static class EventServiceExtension
{
    public static async Task AddEventServices(this IServiceCollection services)
    {
        var transactionCreated = new TransactionCreatedHandler();

        var registry = new DomainEventTypeRegistry();
        registry.Register<TransactionCreatedEvent>(); // typ z Twojej domeny
        var processor = new RabbitMqEventProcessor(registry);

        processor.OnEventReceived += async e =>
        {
            if (e is TransactionCreatedEvent ev)
            {
                await transactionCreated.HandleAsync(ev);
            }
        };

        await processor.StartAsync();
    }
}