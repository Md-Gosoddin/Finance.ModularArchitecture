using BuildingBlock.Domain;

namespace Evently.Module.Consumer.Domain.Consumer;
public sealed class ConsumerEntity : Entity
{
    public Guid ConsumerId { get; set; }
    public string TypeofConsumer { get; set; }
    public static ConsumerEntity Cretae(string typeofcust)
    {
        var @event = new ConsumerEntity
        {
            TypeofConsumer = typeofcust,
            ConsumerId = Guid.NewGuid(),
        };
        @event.Raise(new ConsumerCreatedDomainEvent(@event.ConsumerId));
        return @event;
    }
}
