namespace BuildingBlock.Application.clock;
public interface IDateTimeProvider
{
    DateTime Utcnow { get; }
}
