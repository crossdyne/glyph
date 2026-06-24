using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Query.GetAllGlobalUrls
{
    public static class GetAllGlobalAssetsUrlsEndpoint
    {
        public static void MapGetAllGlobalAssetsUrls(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/global/asset/urls", async ([FromServices] IMediator mediator) =>
            {
                var query = new GetAllGlobalAssetsUrlsQuery();
                var result = await mediator.Send(query);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}