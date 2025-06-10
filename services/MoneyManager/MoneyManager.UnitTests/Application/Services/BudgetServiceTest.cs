using MoneyManager.Application.DTOs;
using MoneyManager.Application.Services;
using MoneyManager.Domain;
using MoneyManager.Domain.Entities;
using MoneyManager.Domain.Events;
using MoneyManager.Domain.GrpcClients;
using MoneyManager.Domain.Repositories;
using Moq;

namespace MoneyManager.UnitTests.Application.Services
{
    [TestFixture]
    public class BudgetServiceTests
    {
        private Mock<IDomainEventDispatcher> _dispatcherMock;
        private Mock<IBudgetRepository> _budgetRepoMock;
        private Mock<IUserGrpcClient> _userClientMock;
        private Mock<IUserPreferencesGrpcClient> _userPreferencesClientMock;
        private BudgetService _budgetService;

        [SetUp]
        public void Setup()
        {
            _dispatcherMock = new Mock<IDomainEventDispatcher>();
            _budgetRepoMock = new Mock<IBudgetRepository>();
            _userClientMock = new Mock<IUserGrpcClient>();

            _budgetService = new BudgetService(_dispatcherMock.Object, _budgetRepoMock.Object, _userClientMock.Object, _userPreferencesClientMock.Object);
        }

        [Test]
        public async Task CreateBudgetAsync_ShouldCreateBudget_WhenUserExists()
        {
            // Arrange
            var dto = new BudgetDto
            {
                Name = "Food Budget",
                Limit = 500,
                CategoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30)
            };

            _userClientMock.Setup(x => x.CheckUserExistsAsync(dto.UserId)).ReturnsAsync(true);

            // Act
            var result = await _budgetService.CreateBudgetAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Limit, Is.EqualTo(dto.Limit));
            Assert.That(result.CategoryId, Is.EqualTo(dto.CategoryId));
            Assert.That(result.UserId, Is.EqualTo(dto.UserId));
            
            _budgetRepoMock.Verify(x => x.AddAsync(It.IsAny<Budget>()), Times.Once);
            _dispatcherMock.Verify(x => x.DispatchAsync(It.IsAny<BudgetCreatedEvent>()), Times.Once);
        }

        [Test]
        public void CreateBudgetAsync_ShouldThrow_WhenUserDoesNotExist()
        {
            // Arrange
            var dto = new BudgetDto
            {
                Name = "Invalid",
                Limit = 100,
                CategoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(10)
            };

            _userClientMock.Setup(x => x.CheckUserExistsAsync(dto.UserId)).ReturnsAsync(false);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _budgetService.CreateBudgetAsync(dto));
            Assert.That(ex.Message, Does.Contain("does not exist"));

            _budgetRepoMock.Verify(x => x.AddAsync(It.IsAny<Budget>()), Times.Never);
            _dispatcherMock.Verify(x => x.DispatchAsync(It.IsAny<BudgetCreatedEvent>()), Times.Never);
        }
    }
}
