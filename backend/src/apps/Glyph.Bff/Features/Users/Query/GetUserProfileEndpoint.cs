using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Users.Query
{
    public static class GetUserProfileEndpoint 
    {
        public static void MapGetUserProfile(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/me", async (
                HttpContext context, 
                [FromServices] IMediator mediator) =>
            {
                var query = new GetUserProfileQuery();
                var result = await mediator.Send(query);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}