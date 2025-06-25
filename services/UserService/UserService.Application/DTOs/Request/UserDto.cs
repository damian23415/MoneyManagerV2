using System.ComponentModel.DataAnnotations;
using UserService.Application.Enums;

namespace UserService.Application.DTOs.Request;

public class UserDto
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessage = "Username jest wymagany.")]
    public string? UserName { get; set; }
    
    [Required]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
    public string? Email { get; set; }
    
    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków.")]
    [MaxLength(100, ErrorMessage = "Hasło może mieć maksymalnie 100 znaków.")]
    public string? Password { get; set; }
    public UserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}