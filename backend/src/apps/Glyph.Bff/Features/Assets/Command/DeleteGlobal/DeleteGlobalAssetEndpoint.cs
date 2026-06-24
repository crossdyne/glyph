using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.DeleteGlobal
{
    public static class DeleteGlobalAssetEndpoint
    {
        public static void MapDeleteGlobalAsset(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/global/asset/{assetId}", async (
                [FromRoute] string assetId, 
                [FromServices] IMediator mediator) =>
            {
                var command = new DeleteGlobalAssetCommand(assetId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            });
        }
    }
}