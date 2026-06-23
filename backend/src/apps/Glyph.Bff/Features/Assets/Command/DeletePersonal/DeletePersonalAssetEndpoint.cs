using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.DeletePersonal
{
    public static class DeletePersonalAssetEndpoint
    {
        public static void MapDeletePersonalAsset(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/personal/asset/{assetId}", async (
                [FromRoute] string assetId, 
                [FromServices] IMediator mediator) =>
            {
                var command = new DeletePersonalAssetCommand(assetId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            });
        }
    }
}