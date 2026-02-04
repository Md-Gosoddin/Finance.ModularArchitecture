
namespace BuildingBlock.Domain;

public abstract class DomainEvent : IDomainEvent
{
    protected DomainEvent()
    {
        GuidId = Guid.NewGuid();
        CreatedateTime = DateTime.UtcNow;
    }
    protected DomainEvent(Guid id, DateTime createdateTime)
    {
        GuidId = id;
        CreatedateTime = createdateTime;
    }
    public Guid GuidId { get; set; }
    public DateTime CreatedateTime { get; set; }
}
