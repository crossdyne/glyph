using Crossdyne.Glyph.Api.Extensions;
using Crossdyne.Glyph.Application.Features.Categories.Commands.CreateGlobal;
using Crossdyne.Glyph.Application.Features.Categories.Commands.DeleteGlobal;
using Crossdyne.Glyph.Application.Features.Categories.Commands.UpdateGlobal;
using Crossdyne.Glyph.Application.Features.Categories.Queries.GetAllGlobal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Crossdyne.Glyph.Api.Controllers
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