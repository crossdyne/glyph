using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.CreatePersonal
{
    public static class CreatePersonalCategoryEndpoint
    {
        public static void MapCreatePersonalCategory(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/personal/category", async (
                [FromBody] CreateCategoryRequest request,
                [FromServices] IMediator mediator) =>
            {                  
                var command = new  CreatePersonalCategoryCommand(request.Name);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).RequireAuthorization();
        }
    }
}