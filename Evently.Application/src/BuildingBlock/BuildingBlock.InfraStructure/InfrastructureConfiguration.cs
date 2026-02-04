using BuildingBlock.Application.clock;
using BuildingBlock.Application.Data;
using BuildingBlock.InfraStructure.clock;
using BuildingBlock.InfraStructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace BuildingBlock.InfraStructure;
public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                         string databaseConnectionString)
    {

        #region ngpSqlDataSource
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        #endregion

        #region Dependency
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        #endregion

        return services;
    }
}

