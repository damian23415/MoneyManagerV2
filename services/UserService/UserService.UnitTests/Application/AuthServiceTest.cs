using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Moq;
using UserService.Application.Services;
using UserService.Application.Services.Interfaces;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.UnitTests.Application;

[TestFixture]
public class AuthServiceTest
{
    private Mock<IPasswordHasher> _passwordHasherMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private AuthService _authService;
    private const string SecretKey = "TestSecretKey12345678901234567890";

    [SetUp]
    public void Setup()
    {
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _authService = new AuthService(_passwordHasherMock.Object, _userRepositoryMock.Object);
    }
    
    
    [Test]
    public async Task LoginAndGetTokenAsync_WhenUserExists_AndPasswordIsValid_ReturnsToken()
    {
        // Arrange
        var testUser = new User 
        { 
            Id = Guid.NewGuid(), 
            Email = "test@test.com",
            PasswordHash = "hashedPassword",
            UserRole = "Admin"
        };
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(testUser);
        _passwordHasherMock.Setup(x => x.VerifyHashedPassword("hashedPassword", "password"))
            .Returns(true);

        // Act
        var result = await _authService.LoginAndGetTokenAsync("test@test.com", "password", SecretKey);

        // Assert
        Assert.That(result, Is.Not.Null);
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(result);
        Assert.Multiple(() =>
        {
            Assert.That(token.Claims.First(c => c.Type == JwtRegisteredClaimNames.Email).Value, Is.EqualTo("test@test.com"));
            Assert.That(token.Claims.First(c => c.Type == ClaimTypes.Role).Value, Is.EqualTo("Admin"));
        });
    }
    
    
    [Test]
    public async Task LoginAndGetTokenAsync_WhenUserDoesNotExist_ReturnsNull()
    {
        // Arrange
        _userRepositoryMock.Setup(x => x.GetByEmailAsync("nonexistent@test.com"))
            .ReturnsAsync((User)null);

        // Act
        var result = await _authService.LoginAndGetTokenAsync("nonexistent@test.com", "password", SecretKey);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task LoginAndGetTokenAsync_WhenPasswordIsInvalid_ReturnsNull()
    {
        // Arrange
        var testUser = new User 
        { 
            Email = "test@test.com",
            PasswordHash = "hashedPassword",
            UserRole = "User"
        };
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync("test@test.com"))
            .ReturnsAsync(testUser);
        _passwordHasherMock.Setup(x => x.VerifyHashedPassword("hashedPassword", "wrongPassword"))
            .Returns(false);

        // Act
        var result = await _authService.LoginAndGetTokenAsync("test@test.com", "wrongPassword", SecretKey);

        // Assert
        Assert.That(result, Is.Null);
    }
}