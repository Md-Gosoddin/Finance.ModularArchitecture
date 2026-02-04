using Evently.Module.User.Application.Repositry;
using Evently.Module.User.Domain.Modules;
using Evently.Module.User.InfraStructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Evently.Module.User.InfraStructure.UserBusiness;
internal sealed class UserRepository(UsersDbContext usersDbContext) : IUserRepository
{
    public void Insert(ClientModules user)
    {
        usersDbContext.Users.Add(user);
    }

    public async Task<ClientModules?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await usersDbContext.Users.SingleOrDefaultAsync(u => u.ClientGuid == id, cancellationToken);

    }
}
