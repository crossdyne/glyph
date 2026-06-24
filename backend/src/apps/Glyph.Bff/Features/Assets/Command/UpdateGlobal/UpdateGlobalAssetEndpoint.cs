using Glyph.Bff.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.UpdateGlobal
{
    public static class UpdateGlobalAssetEndpoint
    {
        public static void MapUpdateGlobalAsset(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/global/asset", async (
                [FromForm] UpdateAssetBffRequest request, 
                [FromServices] IMediator mediator) =>
            {
                await using var fileStream = request.File.OpenReadStream();
                
                var command = new UpdateGlobalAssetCommand(request.AssetId, request.AssetName, fileStream, request.File.FileName, request.CategoryId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}