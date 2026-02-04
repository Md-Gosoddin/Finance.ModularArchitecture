namespace BuildingBlock.Domain;
public interface IDomainEvent
{
    Guid GuidId { get; set; }
    DateTime CreatedateTime { get; set; }
}

