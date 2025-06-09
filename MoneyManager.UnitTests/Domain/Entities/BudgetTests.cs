using MoneyManager.Domain.Entities;

namespace MoneyManager.UnitTests.Domain.Entities;

[TestFixture]
public class BudgetTests
{
    [Test]
    public void Constructor_WithValidArguments_ShouldCreateBudget()
    {
        // Arrange
        var name = "Monthly Budget";
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = DateTime.Now;
        var endDate = startDate.AddMonths(1);

        // Act
        var budget = new Budget(name, limit, categoryId, userId, startDate, endDate);

        // Assert
        Assert.That(budget.Name, Is.EqualTo(name));
        Assert.That(budget.Limit, Is.EqualTo(limit));
        Assert.That(budget.CategoryId, Is.EqualTo(categoryId));
        Assert.That(budget.UserId, Is.EqualTo(userId));
        Assert.That(budget.StartDate, Is.EqualTo(startDate));
        Assert.That(budget.EndDate, Is.EqualTo(endDate));
    }
    
    [TestCase("")]
    [TestCase(" ")]
    public void Constructor_WithInvalidName_ShouldThrow(string invalidName)
    {
        //Arrange
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = DateTime.UtcNow;
        var endDate = startDate.AddMonths(1);
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() => new Budget(invalidName, limit, categoryId, userId, startDate, endDate));

        // Assert
        StringAssert.StartsWith("Budget name cannot be empty", ex.Message);
    }
    
    [TestCase(0)]
    [TestCase(-10)]
    public void Constructor_WithZeroOrNegativeLimit_ShouldThrow(decimal invalidLimit)
    {
        // Arrange
        var name = "Test";
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddMonths(1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() => 
            new Budget(name, invalidLimit, categoryId, userId, startDate, endDate));
        
        //Assert
        StringAssert.StartsWith("Limit can't be 0 or less", ex.Message);
    }
    
    [Test]
    public void Constructor_WithEmptyCategoryId_ShouldThrow()
    {
        // Arrange
        var name = "Test";
        var limit = 1000m;
        var userId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddMonths(1);

        var ex = Assert.Throws<ArgumentException>(() =>
            new Budget(name, limit, Guid.Empty, userId, startDate, endDate));
        
        // Assert
        StringAssert.StartsWith("Category can't be empty", ex.Message);
    }
    
    [Test]
    public void Constructor_WithEmptyUserId_ShouldThrow()
    {
        // Arrange
        var name = "Test";
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddMonths(1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
            new Budget(name, limit, categoryId, Guid.Empty, startDate, endDate));
        
        // Assert
        StringAssert.StartsWith("UserId can't be empty", ex.Message);
    }
    
    [Test]
    public void Constructor_WithEndDateEarlierThanStartDate_ShouldThrow()
    {
        // Arrange
        var name = "Test";
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(-1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
            new Budget(name, limit, categoryId, userId, startDate, endDate));
        
        // Assert
        StringAssert.StartsWith("End date cannot be earlier than start date", ex.Message);
    }
    
    [Test]
    public void Constructor_WithDefaultStartDate_ShouldThrow()
    {
        // Arrange
        var name = "Test";
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = default(DateTime);
        var endDate = DateTime.UtcNow.Date.AddMonths(1);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
            new Budget(name, limit, categoryId, userId, startDate, endDate));
        
        // Assert
        StringAssert.StartsWith("Start date cannot be default value.", ex.Message);
    }
    
    [Test]
    public void Constructor_WithDefaultEndDate_ShouldThrow()
    {
        // Arrange
        var name = "Test";
        var limit = 1000m;
        var categoryId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var startDate = DateTime.UtcNow.Date;
        var endDate = default(DateTime);

        // Act
        var ex = Assert.Throws<ArgumentException>(() =>
            new Budget(name, limit, categoryId, userId, startDate, endDate));
        
        // Assert
        StringAssert.StartsWith("End date cannot be default value.", ex.Message);
    }
}