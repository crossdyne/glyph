using Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal;
using Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal;
using Glyph.Assets.Application.Features.Assets.Commands.UpdateGlobal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Glyph.Assets.Api.Extensions;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Assets.Api.Controllers
{    
    [ApiController]
    [Route("api/v1/global/asset")]
    public class GlobalAssetsController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateGlobalAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            var folders = System.Text.Json.JsonSerializer.Deserialize<List<string>>(request.FoldersJson) ?? new();

            await using var fileStream = file.OpenReadStream();
            
            var command = new CreateGlobalAssetCommand(
                fileStream,
                file.Length,
                request.Bucket, 
                folders,
                request.FileName,
                request.AssetName,
                Guid.Parse(request.CategoryId)); 

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            await using var fileStream = file.OpenReadStream();

            var command = new UpdateGlobalAssetCommand(
                fileStream, 
                file.Length, 
                file.FileName,
                request.AssetName,
                Guid.Parse(request.AssetId));

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteGlobalAssetRequest request)
        {
            var command = new DeleteGlobalAssetCommand(Guid.Parse(request.AssetId));

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }
    }
}