namespace MoneyManager.Domain.Entities;

public class Expense
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }
    public Guid CategoryId { get; private set; }
    
    public Category Category { get; private set; }

    public Expense()
    {
        //for ORM
    }
    
    public Expense(decimal amount, DateTime date, string? description, Guid categoryId)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        
        if (date == default)
            throw new ArgumentException("Date cannot be default value.", nameof(date));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty.", nameof(categoryId));
        
        Amount = amount;
        Date = date;
        Description = description;
        CategoryId = categoryId;
    }
}