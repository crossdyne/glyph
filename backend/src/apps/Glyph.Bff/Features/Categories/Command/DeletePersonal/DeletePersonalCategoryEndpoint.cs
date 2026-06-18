using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.DeletePersonal
{
    public static class DeletePersonalCategoryEndpoint
    {
        public static void MapDeletePersonalCategory(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/personal/category/{categoryId}", async (
                [FromServices] IMediator mediator,
                [FromRoute] string categoryId) =>
            {
                var command = new DeletePersonalCategoryCommand(categoryId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}