using System.Text;
using System.Text.Json;
using MoneyManager.Domain;
using MoneyManager.Domain.Events;
using RabbitMQ.Client;

namespace MoneyManager.Infrastructure;

public class RabbitMqDomainEventDispatcher : IDomainEventDispatcher, IAsyncDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private const string QueueName = "domain-events";
    
    private RabbitMqDomainEventDispatcher(IConnection connection, IChannel channel)
    {
        _connection = connection;
        _channel = channel;
    }
    
    public static async Task<RabbitMqDomainEventDispatcher> CreateAsync()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false);

        return new RabbitMqDomainEventDispatcher(connection, channel);
    }
    
    public async Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IDomainEvent
    {
        var json = JsonSerializer.Serialize(domainEvent, domainEvent.GetType());
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(string.Empty, QueueName, body);
        Console.WriteLine("poszedl dispatch do rabbita");
    }

    public async ValueTask DisposeAsync()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }
}