using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonalAndGlobal
{
    public static class GetAllPersonalAndGlobalCategoriesEndpoint
    {
        public static void MapGetAllPersonalAndGlobalCategories(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/personal/category/all", async (
                HttpContext httpContext, 
                [FromServices] IMediator mediator,
                CancellationToken ct = default) =>
            {
                var query = new GetAllPersonalAndGlobalCategoriesQuery();

                var result = await mediator.Send(query, ct);

                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}