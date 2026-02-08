using BuildingBlock.Application.EventBus;
using BuildingBlock.Application.Messaging;
using Evently.Module.User.Domain.Modules;
using Evently.Module.User.IntegrationEvents;

namespace Evently.Module.User.Application.ClientRepositry;
internal sealed class UserRegisteredDomainEventHandler(IEventBus bus)
                    : DomainEventHandler<UserRegisteredDomainEvent>
{
    public override async Task Handle(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {

        await bus.PublishAsync(
          new UserRegisteredIntegrationEvent(domainEvent.GuidId, domainEvent.CreatedateTime, domainEvent.UserId,
                                             "domainEvent.Email", "domainEvent.UserName"),
          cancellationToken);
    }
}

