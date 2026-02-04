using System.Data.Common;

namespace BuildingBlock.Application.Data;
public interface IDbConnectionFactory
{
    ValueTask<DbConnection> OpenConnectionAsync();
}

