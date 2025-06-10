namespace UserService.Application.DTOs;

public class UserPreferencesDto
{
    public Guid UserId { get; set; }

    public string? PreferredCurrency { get; set; }

    public string? Language { get; set; }

    public bool EmailNotificationsEnabled { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public UserPreferencesDto(Guid userId, string preferredCurrency, string language, bool emailNotificationsEnabled, DateTime createdAt, DateTime? updatedAt)
    {
        UserId = userId;
        PreferredCurrency = preferredCurrency;
        Language = language;
        EmailNotificationsEnabled = emailNotificationsEnabled;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}