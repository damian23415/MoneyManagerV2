using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace MoneyManager.Messaging.RabbitMQ.Publishing;

public class RabbitMqDomainEventPublisher : IDomainEventPublisher, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private const string QueueName = "domain-events";
    
    public RabbitMqDomainEventPublisher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }
    
    public static async Task<RabbitMqDomainEventPublisher> CreateAsync()
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

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

        return new RabbitMqDomainEventPublisher(connection, channel);
    }
    
    public async Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        using var channel = _connection.CreateChannelAsync();

        var json = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(string.Empty, QueueName, body);
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}