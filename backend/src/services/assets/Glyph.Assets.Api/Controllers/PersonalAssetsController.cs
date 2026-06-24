using Glyph.Assets.Application.Features.Assets.Commands.CreatePersonal;
using Glyph.Assets.Application.Features.Assets.Commands.DeletePersonal;
using Glyph.Assets.Application.Features.Assets.Commands.UpdatePersonal;
using Glyph.Assets.Application.Features.Assets.Queries.GetAllByFilter;
using Glyph.Assets.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Glyph.Assets.Application.Features.Assets.Queries.GetMetadata;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Assets.Api.Controllers
{
    [ApiController]
    [Route("api/v1/personal/asset")]
    public sealed class PersonalAssetsController(IMediator mediator) : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] CreateAssetRequest request, [FromForm] IFormFile file)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            var folders = JsonSerializer.Deserialize<List<string>>(request.FoldersJson) ?? [];
            var projectIds = JsonSerializer.Deserialize<List<string>>(request.ProjectIdsJson) ?? [];

            await using var fileStream = file.OpenReadStream();
            
            var command = new CreatePersonalAssetCommand(
                fileStream,
                file.Length,
                request.Bucket, 
                folders,
                request.FileName,
                Guid.Parse(request.CategoryId),
                projectIds,
                request.AssetName,
                extractResult.Value.UserId); 

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Created();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromForm] UpdateAssetRequest request, IFormFile file)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            await using var fileStream = file.OpenReadStream();

            var command = new UpdatePersonalAssetCommand(
                fileStream, 
                file.Length, 
                file.FileName, 
                request.AssetName,
                request.CategoryId,
                Guid.Parse(request.AssetId),
                extractResult.Value.UserId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete("{assetId:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid assetId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new DeletePersonalAssetCommand(extractResult.Value.UserId, assetId);

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllByFiler([FromQuery] Guid projectId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetAllAssetsByFilerQuery(extractResult.Value.UserId, projectId);

            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("metadata/many")]
        [Authorize]
        public async Task<IActionResult> GetMetadata()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetAssetsMetadataQuery(extractResult.Value.UserId);
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}