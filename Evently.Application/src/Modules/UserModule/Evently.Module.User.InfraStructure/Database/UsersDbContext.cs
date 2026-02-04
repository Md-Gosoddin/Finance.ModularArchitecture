using Evently.Module.User.Application.Repositry;
using Evently.Module.User.Domain.Modules;
using Evently.Module.User.InfraStructure.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Evently.Module.User.InfraStructure.Database;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options)
                                                                : DbContext(options), IUnitOfWork
{
    internal DbSet<ClientModules> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Client);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
