using BuildingBlock.Application.EventBus;
using BuildingBlock.InfraStructure.outbox;
using Evently.Module.Consumer.Infrastructure.Database;
using Evently.Module.Consumer.Infrastructure.Inbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Evently.Module.Consumer.Infrastructure;
public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services,
                                                 IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddIntegrationEventHandlers();
        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ConsumerDbContext>((sp, options) =>
     options.UseNpgsql(
             configuration.GetConnectionString("Database"),
             npgsqlOptions => npgsqlOptions
                 .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Consumer))
         .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>())
                .UseSnakeCaseNamingConvention());



        services.Configure<InboxOptions>(configuration.GetSection("Ticketing:Inbox"));

        services.ConfigureOptions<ConfigureProcessInboxJob>();
    }
    private static void AddIntegrationEventHandlers(this IServiceCollection services)
    {

        Type[] integrationEventHandlers = Presentation.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IIntegrationEventHandler)))
            .ToArray();

        foreach (Type integrationEventHandler in integrationEventHandlers)
        {
            services.TryAddScoped(integrationEventHandler);

            Type integrationEvent = integrationEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler =
                typeof(IdempotentIntegrationEventHandler<>).MakeGenericType(integrationEvent);

            services.Decorate(integrationEventHandler, closedIdempotentHandler);
        }
    }
}
