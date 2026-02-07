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
        const string sql =
            """
            SELECT EXISTS(
                SELECT 1
                FROM users.outbox_message_consumers
                WHERE outbox_message_id = @OutboxMessageId AND
                      name = @Name
            )
            """;

        return await dbConnection.ExecuteScalarAsync<bool>(sql, outboxMessageConsumer);
    }

    private static async Task InsertOutboxConsumerAsync(DbConnection dbConnection,
                                                 OutboxMessageConsumer outboxMessageConsumer)
    {
        const string sql =
            """
            INSERT INTO users.outbox_message_consumers(outbox_message_id, name)
            VALUES (@OutboxMessageId, @Name)
            """;

        await dbConnection.ExecuteAsync(sql, outboxMessageConsumer);
    }
}
