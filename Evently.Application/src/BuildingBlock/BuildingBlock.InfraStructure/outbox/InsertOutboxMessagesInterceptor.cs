using BuildingBlock.Domain;
using BuildingBlock.InfraStructure.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace BuildingBlock.InfraStructure.outbox;
public sealed class InsertOutboxMessagesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
                                    InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            InsertOutboxMessages(eventData.Context);
        }
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    private static void InsertOutboxMessages(DbContext context)
    {

        // 1. Grab all entities that have pending domain events
        var entities = context.ChangeTracker.Entries<Entity>().Select(e => e.Entity)
                                                    .Where(e => e.DomainEvents.Any()).ToList();

        // 2. Map those events to OutboxMessages
        var outboxMessages = entities.SelectMany(entity => entity.DomainEvents)
            .Select(domainEvent => new OutboxMessage
            {
                Id = domainEvent.GuidId,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings.Instance),
                OccurredOnUtc = domainEvent.CreatedateTime
            })
            .ToList();

        // 3. Clear the events from entities and save to the Outbox
        entities.ForEach(e => e.RemoveEvent());
        context.Set<OutboxMessage>().AddRange(outboxMessages);
    }
}

