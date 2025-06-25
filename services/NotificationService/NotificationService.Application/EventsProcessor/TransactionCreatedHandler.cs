using Events.Events;
using MoneyManager.Application.EventHandlers;
using NotificationService.Domain.GrpcInterfaces.User;

namespace NotificationService.Application.EventsProcessor;

public class TransactionCreatedHandler : IDomainEventHandler<TransactionCreatedEvent>
{
    private readonly IUserServiceClient _userServiceClient;

    public TransactionCreatedHandler(IUserServiceClient userServiceClient)
    {
        _userServiceClient = userServiceClient;
    }

    public async Task HandleAsync(TransactionCreatedEvent domainEvent)
    {
        var user = await _userServiceClient.GetUserByIdAsync(domainEvent.UserId);
        
        if (user == null)
        {
            Console.WriteLine($"Nie znaleziono użytkownika o ID: {domainEvent.UserId}");
            return;
        }
        
        Console.WriteLine($"Witaj, Dla twojego adresu email {user.Email} została utworzona nowa transakcja!");
    }
}