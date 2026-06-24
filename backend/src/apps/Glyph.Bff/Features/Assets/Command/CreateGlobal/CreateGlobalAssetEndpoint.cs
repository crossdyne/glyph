using Glyph.Bff.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.CreateGlobal
{
    public static class CreateGlobalAssetEndpoint
    {
        public static void MapCreateGlobalAsset(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/global/asset", async (
                [FromForm] CreateAssetBffRequest request, 
                [FromServices] IMediator mediator) =>
            {
                await using var fileStream = request.File.OpenReadStream();

                var command = new CreateGlobalAssetCommand(fileStream, request.File.FileName, request.CategoryId, request.ProjectIdsJson, request.AssetName);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).DisableAntiforgery().RequireAuthorization();   
        }
    }
}