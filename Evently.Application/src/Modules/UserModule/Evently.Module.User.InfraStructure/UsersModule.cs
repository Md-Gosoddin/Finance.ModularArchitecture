using BuildingBlock.InfraStructure.outbox;
using BuildingBlock.Presentation.Endpoint;
using Evently.Module.User.Application.Repositry;
using Evently.Module.User.InfraStructure.Database;
using Evently.Module.User.InfraStructure.UserBusiness;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Module.User.InfraStructure;

public static class UsersModule
{
    public static IServiceCollection AddUsersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        return services;
    }
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>((sp, options) =>
        options.UseNpgsql(configuration.GetConnectionString("Database"), npgsqlOptions =>
        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Client))
         .AddInterceptors(sp.GetRequiredService<InsertOutboxMessagesInterceptor>()));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<UsersDbContext>());
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddEndpoints(Presentation.AssemblyReference.Assembly);
    }
}
