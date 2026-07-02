using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Users
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/me", async (
                HttpContext context, 
                [FromServices] IUserManagementClient client) =>
            {
                var result = await client.Me();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}