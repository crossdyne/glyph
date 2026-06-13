using Glyph.Assets.Application.Features.Assets.Commands.CreatePersonal;
using Glyph.Assets.Application.Features.Assets.Commands.DeletePersonal;
using Glyph.Assets.Application.Features.Assets.Commands.UpdatePersonal;
using Glyph.Assets.Application.Features.Assets.Queries.GetAllByFilter;
using Glyph.Assets.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Glyph.Assets.Api.Controllers
{
    [ApiController]
    [Route("api/v1/personal/asset")]
    public sealed class PersonalAssetsController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreatePersonalAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            var folders = System.Text.Json.JsonSerializer.Deserialize<List<string>>(request.FoldersJson) ?? new();
            var projectIds = System.Text.Json.JsonSerializer.Deserialize<List<string>>(request.ProjectIdsJson) ?? new();

            await using var fileStream = file.OpenReadStream();
            
            var command = new CreatePersonalAssetCommand(
                fileStream,
                file.Length,
                request.Bucket, 
                folders,
                request.FileName,
                Guid.Parse(request.CategoryId),
                projectIds,
                Guid.Parse(request.UserId)); 

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Created();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] UpdatePersonalAssetRequest request, IFormFile file)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Файл не был передан.");

            await using var fileStream = file.OpenReadStream();

            var command = new UpdatePersonalAssetCommand(
                fileStream, 
                file.Length, 
                request.FileName, 
                Guid.Parse(request.AssetId), 
                Guid.Parse(request.UserId));

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeletePersonalAssetRequest request)
        {
            var command = new DeletePersonalAssetCommand(Guid.Parse(request.UserId), Guid.Parse(request.AssetId));

            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllByFiler([FromQuery] Guid userId, [FromQuery] Guid projectId)
        {
            var query = new GetAllAssetsByFilerQuery(userId, projectId);

            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}