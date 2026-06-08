using Crossdyne.Glyph.Api.Extensions;
using Crossdyne.Glyph.Application.Features.Categories.Commands.CreatePersonal;
using Crossdyne.Glyph.Application.Features.Categories.Commands.Delete;
using Crossdyne.Glyph.Application.Features.Categories.Commands.Update;
using Crossdyne.Glyph.Application.Features.Categories.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Crossdyne.Glyph.Api.Controllers
{
    [ApiController]
    [Route("api/v1/personal/category")]
    public sealed class PersonalCategoryController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonalCategoryRequest request)
        {
            var command = new CreatePersonalCategoryCommand(Guid.Parse(request.UserId), request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdatePersonalCategoryRequest request)
        {
            var command = new UpdatePersonalCategoryCommand(Guid.Parse(request.CategoryId), Guid.Parse(request.UserId), request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeletePersonalCategoryRequest request)
        {
            var command = new DeletePersonalCategoryCommand(Guid.Parse(request.CategoryId), Guid.Parse(request.UserId));
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetAll([FromRoute] Guid userId)
        {
            var query = new GetAllPersonalCategoriesQuery(userId);
            var result = await mediator.Send(query);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok(result.Value);
        }
    }
}