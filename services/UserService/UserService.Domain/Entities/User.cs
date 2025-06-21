namespace UserService.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string UserRole { get; set; }

    public User()
    {
        //For Dapper
    }
    
    public User(string userName, string email, string passwordHash, string userRole)
    {
        UserName = userName;
        Email = email;
        PasswordHash = passwordHash;
        UserRole = userRole;
    }
}