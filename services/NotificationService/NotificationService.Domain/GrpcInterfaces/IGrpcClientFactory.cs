using Grpc.Core;

namespace NotificationService.Domain.GrpcInterfaces;

public interface IGrpcClientFactory
{
    T CreateClient<T>() where T : ClientBase<T>;
}