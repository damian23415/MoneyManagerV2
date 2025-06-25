using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using NotificationService.Domain.GrpcInterfaces;

namespace NotificationService.Infrastructure.GrpcClient;

public class GrpcClientFactory : IGrpcClientFactory
{
    private readonly IConfiguration _configuration;
    
    private readonly Dictionary<Type, object> _clientCache = new();

    public GrpcClientFactory(IConfiguration configuration)
    {
        _configuration = configuration;
        AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
    }

    public T CreateClient<T>() where T : ClientBase<T>
    {
        var clientType = typeof(T);

        if (_clientCache.TryGetValue(clientType, out var existing))
            return (T)existing;

        var serviceName = clientType.Name.Replace("Client", string.Empty);

        var baseUrl = _configuration[$"GrpcClients:{serviceName}"];
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException($"Missing gRPC URL for {serviceName} in GrpcClients section.");

        var httpHandler = new SocketsHttpHandler
        {
            EnableMultipleHttp2Connections = true,
            KeepAlivePingDelay = TimeSpan.FromSeconds(30),
            KeepAlivePingTimeout = TimeSpan.FromSeconds(10),
            PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
        };        
        
        var channel = GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
        {
            HttpHandler = httpHandler
        });
        
        var ctor = clientType.GetConstructor(new[] { typeof(GrpcChannel) });
        if (ctor == null)
            throw new InvalidOperationException($"Type {clientType.FullName} doesn't have a GrpcChannel constructor.");
        
        var client = (T)ctor.Invoke(new object[] { channel });
        _clientCache[clientType] = client;

        return client;
    }
}