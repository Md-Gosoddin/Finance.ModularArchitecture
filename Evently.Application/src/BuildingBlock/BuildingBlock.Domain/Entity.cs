namespace BuildingBlock.Domain;
public abstract class Entity : IDomainEvent
{
    public readonly List<IDomainEvent> _events = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.ToList();

    protected void Raise(IDomainEvent domainEvent)
    {
        _events.Add(domainEvent);
    }
    public void RemoveEvent()
    {
        _events.Clear();
    }
    public Guid GuidId { get; set; }
    public DateTime CreatedateTime { get; set; }
}
