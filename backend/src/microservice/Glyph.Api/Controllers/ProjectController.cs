using Glyph.Api.Extensions;
using Glyph.Application.Features.Projects.Commands.Create;
using Glyph.Application.Features.Projects.Commands.Delete;
using Glyph.Application.Features.Projects.Commands.Update;
using Glyph.Application.Features.Projects.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;

namespace Glyph.Api.Controllers
{
    [ApiController]
    [Route("api/v1/project")]
    public sealed class ProjectController(IMediator mediator) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectRequest request)
        {
            var command = new CreateProjectCommand(request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProjectRequest request)
        {
            var command = new UpdateProjectCommand(Guid.Parse(request.ProjectId), request.Name);
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteProjectRequest request)
        {
            var command = new DeleteProjectCommand(Guid.Parse(request.ProjectId));
            var result = await mediator.Send(command);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return NoContent();
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllProjectsQuery();
            var result = await mediator.Send(query);
            
            return Ok(result);
        }
    }
}