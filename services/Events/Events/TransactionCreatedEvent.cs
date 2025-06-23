using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace Events.Events;

public class TransactionCreatedEvent : IDomainEvent
{
    public Guid UserId { get; set; }
    public string Message { get; } = "Twój wydatek został dodany do budżetu.";
    public DateTime OccurredOn { get; } = DateTime.Now;
    public string EventType { get; } = nameof(TransactionCreatedEvent);

    public TransactionCreatedEvent(Guid userId)
    {
        UserId = userId;
    }
}