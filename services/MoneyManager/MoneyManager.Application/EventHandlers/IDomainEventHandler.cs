using MoneyManager.Domain.Events;
using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Application.EventHandlers;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}