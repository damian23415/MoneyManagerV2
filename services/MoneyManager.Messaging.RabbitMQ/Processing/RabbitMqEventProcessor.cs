using System.Text;
using System.Text.Json;
using MoneyManager.Messaging.RabbitMQ.Extensions;
using MoneyManager.Messaging.RabbitMQ.Publishing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MoneyManager.Messaging.RabbitMQ.Processing;

public class RabbitMqEventProcessor : IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly DomainEventTypeRegistry _eventTypeMap;
    private const string QueueName = "domain-events";

    public RabbitMqEventProcessor( DomainEventTypeRegistry eventTypeMap)
    {
        _eventTypeMap = eventTypeMap;
    }

    public event Func<IDomainEvent, Task>? OnEventReceived;

    public async Task StartAsync()
    {
        string rabbitHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
        string rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "guest";
        string rabbitPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "guest";
        
        var factory = new ConnectionFactory
        {
            HostName = rabbitHost,
            UserName = rabbitUser,
            Password = rabbitPass
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            Console.WriteLine(json);

            var eventType = DetectEventType(json);
            var domainEvent = (IDomainEvent)JsonSerializer.Deserialize(json, eventType!)!;
            
            if (OnEventReceived != null)
            {
                await OnEventReceived(domainEvent);
            }
            
            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, CancellationToken.None);
        };

        await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer);
    }

    private Type? DetectEventType(string json)
    {
        using var doc = JsonDocument.Parse(json);
        if (!doc.RootElement.TryGetProperty("EventType", out var typeProperty))
            throw new Exception("EventType missing");

        var eventTypeName = typeProperty.GetString();
        
        return _eventTypeMap.GetEventType(eventTypeName!);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}