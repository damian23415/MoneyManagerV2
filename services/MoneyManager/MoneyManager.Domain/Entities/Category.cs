namespace MoneyManager.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }

    public Category()
    {
    }
    
    public Category(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        Name = name;
    }
}