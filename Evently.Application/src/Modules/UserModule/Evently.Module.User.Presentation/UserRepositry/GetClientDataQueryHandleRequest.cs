using BuildingBlock.Domain;
using BuildingBlock.Presentation.ApiResult;
using BuildingBlock.Presentation.Endpoint;
using Evently.Module.User.Application.ClientRepositry;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Evently.Module.User.Presentation.UserRepositry;
internal sealed class GetClientDataQueryHandleRequest : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("GetclientData", async (ISender sender) =>
        {
            Result<ClientData> @event = await sender.Send(new GetClientDataRequest());
            return @event.Match(Results.Ok, ApiResults.Problem);
        }).WithTags(Tags.client);
    }
}
