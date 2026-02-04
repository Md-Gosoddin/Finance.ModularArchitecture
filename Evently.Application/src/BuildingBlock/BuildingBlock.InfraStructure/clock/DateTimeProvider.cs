using BuildingBlock.Application.clock;

namespace BuildingBlock.InfraStructure.clock;
internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime Utcnow => DateTime.UtcNow;
}
