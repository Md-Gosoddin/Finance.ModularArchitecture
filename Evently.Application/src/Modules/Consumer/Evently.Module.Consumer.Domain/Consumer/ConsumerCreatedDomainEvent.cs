using BuildingBlock.Domain;

namespace Evently.Module.Consumer.Domain.Consumer;

internal sealed class ConsumerCreatedDomainEvent(Guid guid) : DomainEvent
{
    public Guid UserId { get; init; } = guid;
}
