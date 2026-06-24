using Glyph.Bff.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.DeleteGlobal
{
    public static class DeleteGlobalCategoryEndpoint
    {
        public static void MapDeleteGlobalCategory(this IEndpointRouteBuilder app)
        {
            app.MapDelete("api/v1/global/category/{assetId}", async ([FromServices] IMediator mediator, [FromRoute] string assetId) =>
            {
                var command = new DeleteGlobalCategoryCommand(assetId);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();
        }
    }
}