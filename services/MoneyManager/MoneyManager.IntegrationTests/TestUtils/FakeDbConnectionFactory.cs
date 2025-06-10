using System.Data;
using MoneyManager.Infrastructure.Persistence;

namespace MoneyManager.IntegrationTests.TestUtils;

public class FakeDbConnectionFactory : DbConnectionFactory
{
    private readonly IDbConnection _connection;

    public FakeDbConnectionFactory(IDbConnection connection) : base(null!)
    {
        _connection = connection;
    }
    
    public override Task<IDbConnection> CreateOpenConnectionAsync()
    {
        return Task.FromResult(_connection);
    }
}