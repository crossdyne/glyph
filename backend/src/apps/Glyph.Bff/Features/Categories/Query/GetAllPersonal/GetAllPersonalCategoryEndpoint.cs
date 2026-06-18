using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonal
{
    public static class GetAllPersonalCategoryEndpoint
    {
        public static void MapGetAllPersonalCategory(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/personal/category", async (
                HttpContext httpContext, 
                [FromServices] IMediator mediator,
                CancellationToken ct = default) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userId))
                    return Results.Unauthorized(); 

                var query = new GetAllPersonalCategoryQuery(userId);

                var result = await mediator.Send(query, ct);

                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}