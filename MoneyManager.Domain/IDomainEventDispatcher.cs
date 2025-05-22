using MoneyManager.Domain.Events;

namespace MoneyManager.Domain;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}