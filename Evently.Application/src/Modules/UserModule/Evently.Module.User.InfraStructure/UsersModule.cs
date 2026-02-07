using BuildingBlock.Application.Messaging;
using BuildingBlock.InfraStructure.outbox;
using BuildingBlock.Presentation.Endpoint;
using Evently.Module.User.Application.Repositry;
using Evently.Module.User.InfraStructure.Database;
using Evently.Module.User.InfraStructure.Outbox;
using Evently.Module.User.InfraStructure.UserBusiness;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Evently.Module.User.InfraStructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddDomainEventHandlers();
        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>((sp, options) =>
        options.UseNpgsql(configuration.GetConnectionString("Database"), npgsqlOptions =>
        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Client))
         .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
        services.AddScoped<IUserRepository, UserRepository>();
        services.Configure<OutboxOptions>(configuration.GetSection("Users:Outbox"));
        services.ConfigureOptions<ConfigureProcessOutboxJob>();
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
    }
    private static void AddDomainEventHandlers(this IServiceCollection services)
    {

#pragma warning disable S1481 // Unused local variables should be removed
        Type[] allTypes = Application.AssemblyReference.Assembly.GetTypes();
        Console.WriteLine($"Total types found in assembly: {allTypes.Length}");

        foreach (Type? type in allTypes.Where(t => t.Name.Contains("Handler")))
        {
            Type[] interfaces = type.GetInterfaces();
            Console.WriteLine($"Type: {type.Name}");
            foreach (Type @interface in interfaces)
            {
                Console.WriteLine($" - Implements: {@interface.Name} (Generic: {@interface.IsGenericType})");
            }
        }
#pragma warning restore S1481 // Unused local variables should be removed

        Type[] domainEventHandlers = Application.AssemblyReference.Assembly
            .GetTypes()
            .Where(t => t.IsAssignableTo(typeof(IDomainEventHandler)))
            .ToArray();

        foreach (Type domainEventHandler in domainEventHandlers)
        {
            services.TryAddScoped(domainEventHandler);

            Type domainEvent = domainEventHandler
                .GetInterfaces()
                .Single(i => i.IsGenericType)
                .GetGenericArguments()
                .Single();

            Type closedIdempotentHandler = typeof(IdempotentDomainEventHandler<>).MakeGenericType(domainEvent);
            services.Decorate(domainEventHandler, closedIdempotentHandler);
        }
    }
}
