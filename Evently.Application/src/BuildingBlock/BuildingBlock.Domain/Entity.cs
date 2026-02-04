namespace BuildingBlock.Domain;
public abstract class Entity : IDomainEvent
{
    public readonly List<IDomainEvent> _events = [];
    private IReadOnlyCollection<IDomainEvent> _domainEvent => _events.ToList();

    protected void Raise(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
    public void RemoveEvent(IDomainEvent domainEvent)
    {
        _events.Remove(domainEvent);
    }
    public Guid GuidId { get; set; }
    public DateTime CreatedateTime { get; set; }
}
