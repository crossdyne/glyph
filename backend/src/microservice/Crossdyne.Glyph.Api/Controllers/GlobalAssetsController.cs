using Crossdyne.Glyph.Api.Extensions;
using Crossdyne.Glyph.Application.Features.Assets.Commands.CreateGlobal;
using Crossdyne.Glyph.Application.Features.Assets.Commands.DeleteGlobal;
using Crossdyne.Glyph.Application.Features.Assets.Commands.UpdateGlobal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Crossdyne.Glyph.Api.Controllers
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
                Guid.Parse(request.CategoryId)); 

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Created();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] UpdateGlobalAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            await using var fileStream = file.OpenReadStream();

            var command = new UpdateGlobalAssetCommand(
                fileStream, 
                file.Length, 
                request.FileName, 
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