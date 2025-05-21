namespace MoneyManager.Domain.Entities;

public class Budget
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public decimal Limit { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime Month { get; private set; }

    public Category Category { get; private set; }

    // EF Core
    protected Budget() { }

    public Budget(string name, decimal limit, Guid categoryId, DateTime month)
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
        Month = new DateTime(month.Year, month.Month, 1); // tylko miesiąc
    }
}