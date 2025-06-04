using MoneyManager.Application.EventHandlers;
using MoneyManager.Domain.Events;
using MoneyManager.Infrastructure;

var processor = new RabbitMqEventProcessor();

var budgetCreatedHandler = new BudgetCreatedEventHandler();

processor.OnEventReceived += async (domainEvent) =>
{
    switch (domainEvent)
    {
        case BudgetCreatedEvent e:
            await budgetCreatedHandler.HandleAsync(e);
            break;
        // inne eventy i ich handlery...
    }
};

await processor.StartAsync();

Console.WriteLine("Event processor started. Press any key to exit.");
Console.ReadKey();

await processor.DisposeAsync();