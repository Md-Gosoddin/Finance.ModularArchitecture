using BuildingBlock.Application.Messaging;
using BuildingBlock.Domain;
using Evently.Module.User.Application.Repositry;
using Evently.Module.User.Domain.Modules;
using Evently.Module.User.Domain.userConfiguration;

namespace Evently.Module.User.Application.ClientRepositry;

public sealed record GetClientDataRequest : IQuery<ClientData>;
public sealed record ClientData(IEnumerable<ClientModules> Modules);
internal sealed class GetClientDataQueryHandle(IUserRepository userRepository) : IQueryHandler<GetClientDataRequest, ClientData>
{
    public async Task<Result<ClientData>> Handle(GetClientDataRequest request, CancellationToken cancellationToken)
    {
        ICollection<ClientModules> data = await userRepository.GetDat(cancellationToken);
        if (data is null)
        {
            return Result.Failure<ClientData>(UserErrors.DataNotAvailable);
        }
        return Result.Success(new ClientData(data));
    }
}



