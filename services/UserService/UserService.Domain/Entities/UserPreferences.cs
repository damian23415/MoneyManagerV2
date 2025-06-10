namespace UserService.Domain.Entities;

public class UserPreferences
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }

    public string PreferredCurrency { get; set; } = "PLN";

    public string Language { get; set; } = "pl";

    public bool EmailNotificationsEnabled { get; set; } = false;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public User User { get; set; }
}