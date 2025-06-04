namespace MoneyManager.Domain.Events;

public class BudgetCreatedEvent : IDomainEvent
{
    public Guid BudgetId { get; }
    public string Name { get; }
    public decimal InitialAmount { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string EventType { get; } = nameof(BudgetCreatedEvent);

    public BudgetCreatedEvent(Guid budgetId, string name, decimal initialAmount)
    {
        BudgetId = budgetId;
        Name = name;
        InitialAmount = initialAmount;
    }
}