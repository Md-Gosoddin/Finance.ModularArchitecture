using System.Data.Common;
using BuildingBlock.InfraStructure.Inbox;
using BuildingBlock.InfraStructure.outbox;
using Evently.Module.Consumer.Domain.Consumer;
using Evently.Module.Consumer.Infrastructure.Consumer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Evently.Module.Consumer.Infrastructure.Database;
public sealed class ConsumerDbContext(DbContextOptions<ConsumerDbContext> options)
    : DbContext(options)
{
    internal DbSet<ConsumerEntity> Customers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Consumer);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConsumerConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConfiguration());
        modelBuilder.ApplyConfiguration(new InboxMessageConsumerConfiguration());
    }
    public async Task<DbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (Database.CurrentTransaction is not null)
        {
            await Database.CurrentTransaction.DisposeAsync();
        }

        return (await Database.BeginTransactionAsync(cancellationToken)).GetDbTransaction();
    }


}
