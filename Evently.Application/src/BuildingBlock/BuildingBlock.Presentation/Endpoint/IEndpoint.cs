using Microsoft.AspNetCore.Routing;

namespace BuildingBlock.Presentation.Endpoint;
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}


