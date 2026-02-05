using BuildingBlock.Domain;
using BuildingBlock.Presentation.ApiResult;
using BuildingBlock.Presentation.Endpoint;
using Evently.Module.User.Application.ClientRepositry;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
namespace Evently.Module.User.Presentation.UserRepositry;
internal sealed class CreateUserCommandHadlerRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("client/create", async (ClintData data, ISender sender) =>
        {
            Result<CommandHandlerResponse> @event = await sender.Send(new CreateCommnadHandlerRequest(data.username, data.email, data.identity));
            return @event.Match(Results.Ok, ApiResults.Problem);
        }).WithTags(Tags.client);
    }
}

public class ClintData
{
    public string username { get; set; }
    public string email { get; set; }
    public string identity { get; set; }
}
