using MoneyManager.Domain.Events;

namespace MoneyManager.Application.EventHandlers;

public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
{
    Task HandleAsync(TEvent domainEvent);
}