namespace MoneyManager.Domain.Entities;

public class Budget
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public decimal Limit { get; private set; }
    public Guid CategoryId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime StartDate { get; private set; } = DateTime.UtcNow;
    public DateTime EndDate { get; private set; }

    public Category Category { get; private set; }

    public Budget()
    {
    }
    
    public Budget(string name, decimal limit, Guid categoryId, Guid userId, DateTime startDate, DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Budget name cannot be empty.", nameof(name));

        if (limit is 0 or <= 0)
            throw new ArgumentException("Limit can't be 0 or less", nameof(limit));
        
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category can't be empty", nameof(categoryId));
        
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId can't be empty", nameof(userId));
        
        if (EndDate < StartDate)
            throw new ArgumentException("End date cannot be earlier than start date.", nameof(EndDate));
        
        if (StartDate == default)
            throw new ArgumentException("Start date cannot be default value.", nameof(StartDate));
        
        if (EndDate == default)
            throw new ArgumentException("End date cannot be default value.", nameof(EndDate));

        Name = name;
        Limit = limit;
        CategoryId = categoryId;
        UserId = userId;
        StartDate = startDate;
        EndDate = endDate;
    }
}