
namespace MoneyManager.Messaging.RabbitMQ.Publishing;

public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent;
}