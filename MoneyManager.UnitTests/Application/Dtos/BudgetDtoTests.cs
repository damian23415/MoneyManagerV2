using System.ComponentModel.DataAnnotations;
using MoneyManager.Application.DTOs;

namespace MoneyManager.UnitTests.Application.Dtos;

[TestFixture]
public class BudgetDtoTests
{
    private bool ValidateModel(BudgetDto dto, out List<ValidationResult> results)
    {
        var context = new ValidationContext(dto, null, null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(dto, context, results, true);
    }

    [Test]
    public void Should_Be_Valid_When_All_Properties_Are_Correct()
    {
        var dto = new BudgetDto
        {
            UserId = Guid.NewGuid(),
            Name = "Test Budget",
            Limit = 100,
            CategoryId = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(10)
        };

        var isValid = ValidateModel(dto, out var results);

        Assert.IsTrue(isValid);
        Assert.IsEmpty(results);
    }

    [Test]
    public void Should_Fail_When_Name_Is_Too_Short()
    {
        var dto = new BudgetDto
        {
            UserId = Guid.NewGuid(),
            Name = "AB",
            Limit = 100,
            CategoryId = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(10)
        };

        var isValid = ValidateModel(dto, out var results);

        Assert.IsFalse(isValid);
        Assert.IsTrue(results.Exists(r => r.ErrorMessage.Contains("Name must be between 3 and 100 characters.")));
    }

    [Test]
    public void Should_Fail_When_Limit_Is_Negative()
    {
        var dto = new BudgetDto
        {
            UserId = Guid.NewGuid(),
            Name = "Valid Name",
            Limit = -10,
            CategoryId = Guid.NewGuid(),
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(10)
        };

        var isValid = ValidateModel(dto, out var results);

        Assert.IsFalse(isValid);
        Assert.IsTrue(results.Exists(r => r.ErrorMessage.Contains("Limit must be a positive number.")));
    }
}