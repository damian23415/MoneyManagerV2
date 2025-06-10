using System.Data;
using Dapper;

namespace MoneyManager.IntegrationTests.TestUtils;

public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
{
    public override void SetValue(IDbDataParameter parameter, Guid value)
        => parameter.Value = value.ToString();

    public override Guid Parse(object value)
    {
        if (value is string s && Guid.TryParse(s, out var guid))
            return guid;

        throw new DataException("Nie można przekonwertować wartości na Guid.");
    }
}