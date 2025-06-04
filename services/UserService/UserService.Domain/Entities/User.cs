namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    public User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }
}