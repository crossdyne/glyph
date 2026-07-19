using Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal;
using Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal;
using Glyph.Assets.Application.Features.Assets.Commands.UpdateGlobal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;
using Shared.Contracts.Assets.Requests;
using Microsoft.AspNetCore.Authorization;
using Glyph.Assets.Api.Constants;
using Glyph.Assets.Application.Features.Assets.Queries.GetGlobalMetadata;
using System.Text.Json;

namespace Glyph.Assets.Api.Controllers
{    
    [ApiController]
    [Route("api/v1/global/asset")]
    [Authorize(PolicyConstants.AdminOnly)]
    public class GlobalAssetsController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateGlobalAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            var folders = JsonSerializer.Deserialize<List<string>>(request.FoldersJson) ?? [];
            var projectIds = JsonSerializer.Deserialize<List<string>>(request.ProjectIdsJson) ?? [];

            await using var fileStream = file.OpenReadStream();
            
            var command = new CreateGlobalAssetCommand(
                fileStream,
                file.Length,
                request.Bucket, 
                folders,
                request.FileName,
                Guid.Parse(request.CategoryId),
                projectIds,
                request.AssetName); 

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateAssetRequest request, IFormFile? file = null)
        {
            await using var fileStream = file?.OpenReadStream();

            var command = new UpdateGlobalAssetCommand(
                fileStream, 
                file?.Length, 
                file?.FileName, 
                request.AssetName,
                request.CategoryId,
                Guid.Parse(request.AssetId));

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete("{assetId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid assetId)
        {
            var command = new DeleteGlobalAssetCommand(assetId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }
        
        [HttpGet("metadata/many")]
        public async Task<IActionResult> GetMetadata()
        {
            var query = new GetGlobalMetadataQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}