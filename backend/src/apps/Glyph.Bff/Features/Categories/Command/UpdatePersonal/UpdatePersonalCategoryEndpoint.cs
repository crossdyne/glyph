using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.UpdatePersonal
{
    public static class UpdatePersonalCategoryEndpoint
    {
        public static void MapUpdatePersonalCategory(this IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/personal/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromBody] UpdateCategoryRequest request,
                [FromServices] IMediator mediator) =>
            {
                var command = new UpdatePersonalCategoryCommand(categoryId, request.Name);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            });
        }
    }
}