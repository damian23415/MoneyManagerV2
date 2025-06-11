namespace MoneyManager.Messaging.RabbitMQ.Publishing;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    string EventType { get; }
}