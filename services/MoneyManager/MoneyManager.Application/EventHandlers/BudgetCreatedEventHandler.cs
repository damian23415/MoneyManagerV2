using MoneyManager.Domain.Events;

namespace MoneyManager.Application.EventHandlers;

public class BudgetCreatedEventHandler : IDomainEventHandler<BudgetCreatedEvent>
{
    public Task HandleAsync(BudgetCreatedEvent domainEvent)
    {
        Console.WriteLine($"Budżet stworzony: {domainEvent.Name}, kwota: {domainEvent.InitialAmount}");
        // tu np. wysyłaj maila, loguj, aktualizuj inne moduły

        return Task.CompletedTask;
    }
}