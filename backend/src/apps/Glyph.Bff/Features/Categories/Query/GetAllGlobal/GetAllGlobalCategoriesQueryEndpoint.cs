using Glyph.Bff.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Bff.Features.Categories.Query.GetAllGlobal
{
    public static class GetAllGlobalCategoriesQueryEndpoint
    {
        public static void MapGetAllGlobalCategories(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/global/category", async ([FromServices] IMediator mediator) =>
            {
                var query = new GetAllGlobalCategoriesQuery();
                var result = await mediator.Send(query);

                return Results.Ok(result);
            }).RequireAuthorization();
        }   
    }
}