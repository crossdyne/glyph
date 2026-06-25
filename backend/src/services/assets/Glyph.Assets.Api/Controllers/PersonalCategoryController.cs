using Glyph.Assets.Application.Features.Categories.Commands.CreatePersonal;
using Glyph.Assets.Application.Features.Categories.Commands.DeletePersonal;
using Glyph.Assets.Application.Features.Categories.Commands.UpdatePersonal;
using Glyph.Assets.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Glyph.Assets.Application.Features.Categories.Queries.GetAllPersonal;
using Shared.Contracts.Assets.Requests;

namespace Glyph.Assets.Api.Controllers
{
    [ApiController]
    [Route("api/v1/personal/category")]
    public sealed class PersonalCategoryController(IMediator mediator) : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new CreatePersonalCategoryCommand(extractResult.Value.UserId, request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok(result.Value);
        }

        [HttpPatch("{categoryId:guid}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new UpdatePersonalCategoryCommand(categoryId, extractResult.Value.UserId, request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete("{categoryId:guid}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var command = new DeletePersonalCategoryCommand(categoryId, extractResult.Value.UserId);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetAllPersonalCategoriesQuery(extractResult.Value.UserId);
            var result = await mediator.Send(query);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok(result.Value);
        }
    }
}