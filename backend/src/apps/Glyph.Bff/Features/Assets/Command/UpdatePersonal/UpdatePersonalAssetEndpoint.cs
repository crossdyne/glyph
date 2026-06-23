using Glyph.Bff.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.UpdatePersonal
{
    public static class UpdatePersonalAssetEndpoint
    {
        public static void MapUpdatePersonalAsset(this IEndpointRouteBuilder app)
        {
            app.MapPut("api/v1/personal/asset", async (
                [FromForm] UpdateAssetBffRequest request, 
                [FromServices] IMediator mediator) =>
            {
                await using var fileStream = request.File.OpenReadStream();
                
                var command = new UpdatePersonalAssetCommand(request.AssetId, request.AssetName, fileStream, request.File.FileName, request.CategoryId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}