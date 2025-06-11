using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Messaging.RabbitMQ.Extensions;

public class DomainEventTypeRegistry
{
    private readonly Dictionary<string, Type> _eventTypeMap = new();

    public void Register<TEvent>() where TEvent : IDomainEvent
    {
        var eventTypeName = typeof(TEvent).Name;
        _eventTypeMap[eventTypeName] = typeof(TEvent);
    }

    public Type? GetEventType(string eventTypeName)
    {
        _eventTypeMap.TryGetValue(eventTypeName, out var type);
        return type;
    }
}
