using Events.Events;
using MoneyManager.Application.EventHandlers;

namespace NotificationService.Application.EventsProcessor;

public class TransactionCreatedHandler : IDomainEventHandler<TransactionCreatedEvent>
{
    public async Task HandleAsync(TransactionCreatedEvent domainEvent)
    {
        Console.WriteLine("Wyślij powiadomienie o nowej transakcji");
    }
}