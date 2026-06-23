using Glyph.Assets.Application.Features.Categories.Commands.CreateGlobal;
using Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal;
using Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal;
using Glyph.Assets.Application.Features.Categories.Queries.GetAllGlobal;
using Glyph.Assets.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Assets.Api.Controllers
{    
    [ApiController]
    [Route("api/v1/global/category")]
    public sealed class GlobalCategoryController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateGlobalCategoryCommand(request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpPatch("{categoryId:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateGlobalCategoryCommand(categoryId, request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete("{categoryId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId)
        {
            var command = new DeleteGlobalCategoryCommand(categoryId);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllGlobalCategoriesQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }
    }
}