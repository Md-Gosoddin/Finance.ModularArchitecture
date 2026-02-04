using System.Data.Common;
using BuildingBlock.Application.Data;
using Npgsql;

namespace BuildingBlock.InfraStructure.Data;
internal sealed class DbConnectionFactory(NpgsqlDataSource dataSource) : IDbConnectionFactory
{
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await dataSource.OpenConnectionAsync();
    }
}
