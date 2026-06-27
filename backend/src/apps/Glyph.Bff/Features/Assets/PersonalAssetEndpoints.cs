using Glyph.Bff.Constants;
using Glyph.Bff.Contracts.Requests;
using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets
{
    public static class PersonalAssetEndpoints
    {
        public static void MapPersonalAssetEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/personal/asset", async (
                [FromForm] CreateAssetBffRequest request, 
                [FromServices] IPersonalAssetClient client) =>
            {
                await using var fileStream = request.File.OpenReadStream();

                var result = await client.Create(
                FileStorageConstants.Bucket, 
                FileStorageConstants.PersonalAssetsSvgFolders, 
                request.File.FileName, 
                request.CategoryId, 
                request.ProjectIdsJson, 
                fileStream,
                request.AssetName).CatchAsync();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).DisableAntiforgery().RequireAuthorization();  

            app.MapDelete("api/v1/personal/asset/{assetId}", async (
                [FromRoute] string assetId, 
                [FromServices] IPersonalAssetClient client) =>
            {
                var result = await client.DeleteAsync(assetId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapPut("api/v1/personal/asset", async (
                [FromForm] UpdateAssetBffRequest request, 
                [FromServices] IPersonalAssetClient client) =>
            {
                await using var fileStream = request.File.OpenReadStream();

                var result = await client.UpdateAsync(request.AssetId, request.AssetName, fileStream, request.File.FileName, request.CategoryId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).DisableAntiforgery().RequireAuthorization();
        }
    }
}