using System.ComponentModel.DataAnnotations;
using BuildingBlock.Domain;

namespace Evently.Module.User.Domain.Modules;
public sealed class ClientModules : Entity
{
    [Key]
    public Guid ClientGuid { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string IdentityId { get; private set; }

    public static ClientModules Create(Guid userId, string email)
    {
        var @event = new ClientModules
        {
            ClientGuid = userId,
            Email = email
        };

        @event.Raise(new UserRegisteredDomainEvent(@event.ClientGuid));
        return @event;
    }
}

public sealed class UserRegisteredDomainEvent(Guid userId) : DomainEvent
{
    public Guid UserId { get; init; } = userId;
}
