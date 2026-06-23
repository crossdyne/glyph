using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Projects.Query.GetAll
{
    public static class GetAllProjectsEndpoint
    {
        public static void MapGetAllProjects(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/project", async ([FromServices] IMediator mediator) =>
            {
                var query = new GetAllProjectsQuery();
                List<ProjectResponse> result = await mediator.Send(query);

                return Results.Ok(result);
            });
        }
    }
}