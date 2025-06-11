using MoneyManager.Messaging.RabbitMQ.Publishing;

namespace MoneyManager.Domain.Events;

public class BudgetExceededEvent : IDomainEvent
{
    public Guid UserId { get; }
    public Guid BudgetId { get; }
    public string CategoryName { get; }
    public decimal InitialAmount { get; }
    public decimal ActualAmount { get; }
    
    public DateTime OccurredOn { get; } = DateTime.Now;
    public string EventType { get; } = nameof(BudgetExceededEvent);

    public BudgetExceededEvent(Guid userId, Guid budgetId, string categoryName, decimal initialAmount, decimal actualAmount)
    {
        UserId = userId;
        BudgetId = budgetId;
        CategoryName = categoryName;
        InitialAmount = initialAmount;
        ActualAmount = actualAmount;
    }
}