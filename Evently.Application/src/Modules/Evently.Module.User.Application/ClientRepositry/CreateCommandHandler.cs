using BuildingBlock.Application.Messaging;
using BuildingBlock.Domain;
using Evently.Module.User.Application.Repositry;
using Evently.Module.User.Domain.Modules;
using Microsoft.Extensions.Logging;

namespace Evently.Module.User.Application.ClientRepositry;

public sealed record CreateCommnadHandlerRequest(string UserName, string Email, string Identity) :
                                                         ICommand<CommandHandlerResponse>;
public sealed record CommandHandlerResponse(Guid id);
internal sealed class CreateCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository,
    ILogger<CreateCommandHandler> logger) : ICommandHandler<CreateCommnadHandlerRequest, CommandHandlerResponse>
{
    public async Task<Result<CommandHandlerResponse>> Handle(CreateCommnadHandlerRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Entered into create Client Module");
        var @event = ClientModules.Create(request.UserName, request.Email, request.Identity);
        userRepository.Insert(@event);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(new CommandHandlerResponse(@event.GuidId));
    }
}
