using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Bff.Features.Projects
{
    public static class ProjectEndpoints
    {
        public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/project", async ([FromServices] IProjectClient client) => Results.Ok(await client.GetAllAsync()));
        }
    }
}