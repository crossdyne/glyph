using Glyph.Bff.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.UpdateGlobal
{
    public static class UpdateGlobalCategoryEndpoint
    {
        public static void MapUpdateGlobalCategory(this IEndpointRouteBuilder app)
        {
            app.MapPatch("api/v1/global/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromBody] UpdateCategoryRequest request,
                [FromServices] IMediator mediator) =>
            {
                var command = new UpdateGlobalCategoryCommand(categoryId, request.Name);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}