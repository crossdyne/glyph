using Glyph.Assets.Application.Features.Categories.Commands.CreateGlobal;
using Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal;
using Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal;
using Glyph.Assets.Application.Features.Categories.Queries.GetAllGlobal;
using Glyph.Assets.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Glyph.Assets.Api.Controllers
{    
    [ApiController]
    [Route("api/v1/global/category")]
    public sealed class GlobalCategoryController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGlobalCategoryRequest request)
        {
            var command = new CreateGlobalCategoryCommand(request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateGlobalCategoryRequest request)
        {
            var command = new UpdateGlobalCategoryCommand(Guid.Parse(request.CategoryId), request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteGlobalCategoryRequest request)
        {
            var command = new DeleteGlobalCategoryCommand(Guid.Parse(request.CategoryId));
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