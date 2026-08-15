using Glyph.Bff.Constants;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets
{
    public static class GlobalAssetEndpoints
    {
        public static void MapPersonalAssetEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/global/asset", async (
                [FromForm] string categoryId,
                [FromForm] string projectIdsJson,
                [FromForm] IFormFile file,
                [FromForm] string assetName,
                [FromServices] IGlobalAssetClient client) =>
            {
                await using var fileStream = file.OpenReadStream();

                var result = await client.Create(
                    FileStorageConstants.Bucket, 
                    FileStorageConstants.GlobalAssetsSvgFolders, 
                    file.FileName, 
                    categoryId, 
                    projectIdsJson, 
                    fileStream,
                    assetName).CatchAsync();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).DisableAntiforgery().RequireAuthorization(); 

            app.MapDelete("api/v1/global/asset/{assetId}", async (
                [FromRoute] string assetId, 
                [FromServices] IGlobalAssetClient client) =>
            {
                var result = await client.DeleteAsync(assetId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapPut("api/v1/global/asset", async (
                [FromForm] string assetId,
                [FromForm] string assetName,
                [FromForm] string categoryId,
                [FromForm] IFormFile? file, 
                [FromServices] IGlobalAssetClient client) =>
            {
                await using var fileStream = file?.OpenReadStream();

                var result = await client.UpdateAsync(assetId, assetName, fileStream, file?.FileName, categoryId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}