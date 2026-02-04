using Evently.Module.User.Domain.Modules;

namespace Evently.Module.User.Application.Repositry;
public interface IUserRepository
{
    Task<ClientModules?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    void Insert(ClientModules user);
}
