using BuildingBlock.Application.EventBus;
using MassTransit;

namespace BuildingBlock.InfraStructure.EventBusManag;

internal sealed class EventBus(IBus bus) : IEventBus
{
    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await bus.Publish(integrationEvent, cancellationToken);
    }
}
