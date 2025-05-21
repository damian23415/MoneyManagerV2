using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Application.EventHandlers;
using MoneyManager.Domain.Events;

namespace MoneyManager.Application.Services;

public interface IDomainEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
        
        foreach (var handler in handlers)
            await handler.HandleAsync(domainEvent);
    }
}