namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string UserRole { get; set; }

    public User(string userName, string email, string passwordHash, string userRole)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        UserRole = userRole;
    }
}