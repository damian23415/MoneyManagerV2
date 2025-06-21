using Moq;
using UserService.Application.DTOs;
using UserService.Application.Services;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;

namespace UserService.UnitTests.Application;

[TestFixture]
public class UserPreferencesTest
{
    private Mock<IUserPreferencesRepository> _userPreferencesRepositoryMock;
    
    [SetUp]
    public void Setup()
    {
        _userPreferencesRepositoryMock = new Mock<IUserPreferencesRepository>();
    }
    
    [Test]
    public async Task GetUserPreferencesAsync_WhenUserPreferencesExist_ReturnsPreferences()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var domainPreferences = new UserPreferences
        {
            UserId = userId,
            PreferredCurrency = "USD",
            Language = "en-US",
            EmailNotificationsEnabled = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        var expectedDto = new UserPreferencesDto(
            userId,
            domainPreferences.PreferredCurrency,
            domainPreferences.Language,
            domainPreferences.EmailNotificationsEnabled,
            domainPreferences.CreatedAt,
            domainPreferences.UpdatedAt
        );
        
        _userPreferencesRepositoryMock
            .Setup(repo => repo.GetUserPreferencesAsync(userId))
            .ReturnsAsync(domainPreferences);
        
        var service = new UserPreferencesService(_userPreferencesRepositoryMock.Object);
        
        // Act
        var result = await service.GetUserPreferencesAsync(userId);
        
        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.UserId, Is.EqualTo(expectedDto.UserId));
        Assert.That(result.PreferredCurrency, Is.EqualTo(expectedDto.PreferredCurrency));
        Assert.That(result.Language, Is.EqualTo(expectedDto.Language));
        Assert.That(result.EmailNotificationsEnabled, Is.EqualTo(expectedDto.EmailNotificationsEnabled));
    }
    
    [Test]
    public async Task GetUserPreferencesAsync_WhenUserPreferencesNotExist_ReturnsPreferences()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        
        _userPreferencesRepositoryMock
            .Setup(repo => repo.GetUserPreferencesAsync(userId))
            .ReturnsAsync((UserPreferences)null);
        
        var service = new UserPreferencesService(_userPreferencesRepositoryMock.Object);
        
        // Act
        var result = await service.GetUserPreferencesAsync(userId);
        
        // Assert
        Assert.That(result, Is.Null);
    }
}