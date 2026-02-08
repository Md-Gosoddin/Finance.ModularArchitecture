using System.Data.Common;
using BuildingBlock.Application.Data;
using BuildingBlock.Application.Messaging;
using BuildingBlock.Domain;
using BuildingBlock.InfraStructure.outbox;
using Dapper;

namespace Evently.Module.User.InfraStructure.Outbox;
internal sealed class IdempotentDomainEventHandler<TDomainEvent>(IDomainEventHandler<TDomainEvent> decorated,
                     IDbConnectionFactory dbConnectionFactory) : DomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    public override async Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var outboxMessageConsumer = new OutboxMessageConsumer(domainEvent.GuidId, decorated.GetType().Name);

        if (await OutboxConsumerExistsAsync(connection, outboxMessageConsumer))
        {
            return;
        }
        await decorated.Handle(domainEvent, cancellationToken);
        await InsertOutboxConsumerAsync(connection, outboxMessageConsumer);
    }
    private static async Task<bool> OutboxConsumerExistsAsync(DbConnection dbConnection,
                                                    OutboxMessageConsumer outboxMessageConsumer)
    {
        try
        {
            const string sql =
                       """
            SELECT EXISTS(
                SELECT 1
                FROM "Client".outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND
                      name = @Name
            )
            """;
            bool resl = await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
            return resl;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return false;
        }
    }

    private static async Task InsertOutboxConsumerAsync(DbConnection dbConnection,
                                                 OutboxMessageConsumer outboxMessageConsumer)
    {
        try
        {
            const string sql =
                """
            INSERT INTO "Client".outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;
            int res = await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
            Console.WriteLine(res);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

        }
    }
}
