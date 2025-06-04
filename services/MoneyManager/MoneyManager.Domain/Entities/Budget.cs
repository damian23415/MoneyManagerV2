namespace MoneyManager.Domain.Entities;

public class Budget
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public decimal Limit { get; private set; }
    public Guid CategoryId { get; private set; }

    public Category Category { get; private set; }

    public Budget()
    {
    }
    
    public Budget(string name, decimal limit, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name cannot be empty.", nameof(name));

        if (limit is 0 or <= 0)
            throw new ArgumentException("Limit can't be 0 or less", nameof(name));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category can't be empty", nameof(name));

        Name = name;
        Limit = limit;
        CategoryId = categoryId;
    }
}