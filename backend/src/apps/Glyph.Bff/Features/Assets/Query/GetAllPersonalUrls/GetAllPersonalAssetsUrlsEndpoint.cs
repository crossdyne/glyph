using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Query.GetAllPersonalUrls
{
    public static class GetAllPersonalAssetsUrlsEndpoint
    {
        public static void MapGetAllPersonalAssetsUrls(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/v1/personal/asset/urls", async ([FromServices] IMediator mediator) =>
            {
                var query = new GetAllPersonalAssetsUrlsQuery();
                var result = await mediator.Send(query);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            });
        }
    }
}