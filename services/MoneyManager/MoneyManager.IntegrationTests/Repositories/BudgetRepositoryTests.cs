using System.Data;
using Dapper;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using MoneyManager.Domain.Entities;
using MoneyManager.Infrastructure.Repositories;
using MoneyManager.IntegrationTests.TestUtils;
using NUnit.Framework;

namespace MoneyManager.IntegrationTests.Repositories;

[TestFixture]
public class BudgetRepositoryTests
{
    private IDbConnection _connection;
    private BudgetRepository _budgetRepository;
    
    [SetUp]
    public void SetUp()
    {
        // InMemory SQLite
        var sqliteConnection = new SqliteConnection("DataSource=:memory:");
        sqliteConnection.Open();
        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        // Utwórz schemat bazy (dopasuj kolumny do swojej klasy Budget!)
        sqliteConnection.Execute(@"
            CREATE TABLE Budgets (
                Id uniqueidentifier PRIMARY KEY,
                Name nvarchar(100) NOT NULL,
                [Limit] decimal NOT NULL,
                CategoryId uniqueidentifier NOT NULL,
                UserId uniqueidentifier NOT NULL,
                StartDate datetime NULL,
                EndDate datetime NULL
            );
        ");
        var factory = new FakeDbConnectionFactory(sqliteConnection);

        _connection = sqliteConnection;
        _budgetRepository = new BudgetRepository(factory);
    }
    
    [TearDown]
    public void TearDown()
    {
        _connection.Close();
    }

    [Test]
    public async Task AddAndRetrieveBudget_ShouldReturnCorrectBudget()
    {
        var budget = new Budget("Test Budget", 100.0m, Guid.NewGuid(), Guid.NewGuid(), DateTime.Now,
            DateTime.Now.AddMonths(1));

        await _budgetRepository.AddAsync(budget);
        var result = await _budgetRepository.GetBudgetForUserAndCategoryAsync(budget.UserId, budget.CategoryId);

        result.Should().NotBeNull();
        result.Name.Should().Be("Test Budget");
        result.Limit.Should().Be(100.0m);
    }
    
    [Test]
    public async Task GetBudgetForUserAndCategoryAsync_WhenNoBudgetExists_ShouldReturnNull()
    {
        var result = await _budgetRepository.GetBudgetForUserAndCategoryAsync(Guid.NewGuid(), Guid.NewGuid());
        result.Should().BeNull();
    }
}