using BuildingBlock.Application.clock;
using BuildingBlock.Application.Data;
using BuildingBlock.Application.EventBus;
using BuildingBlock.InfraStructure.clock;
using BuildingBlock.InfraStructure.Data;
using BuildingBlock.InfraStructure.outbox;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Quartz;

namespace BuildingBlock.InfraStructure;
public static class InfrastructureConfiguration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
                                     Action<IRegistrationConfigurator>[] moduleConfigureConsumers, string databaseConnectionString)
    {

        #region ngpSqlDataSource
        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        services.TryAddSingleton(npgsqlDataSource);
        #endregion

        #region Quartz Services
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        #endregion

        #region massTx
        services.AddMassTransit(configure =>
        {
            foreach (Action<IRegistrationConfigurator> configureConsumers in moduleConfigureConsumers)
            {
                configureConsumers(configure);
            }

            configure.SetKebabCaseEndpointNameFormatter();

            configure.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });
        #endregion


        #region Dependency
        services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
        services.TryAddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.TryAddSingleton<InsertOutboxMessagesInterceptor>();
        services.TryAddSingleton<IEventBus, EventBusManag.EventBus>();

        #endregion

        return services;
    }
}

