using Glyph.Bff.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories.Command.CreateGlobal
{
    public static class CreateGlobalCategoryEndpoint
    {
        public static void MapCreateGlobalCategory(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/global/category", async ([FromServices] IMediator mediator, [FromBody] CreateCategoryRequest request) =>
            {
                var command = new CreateGlobalCategoryCommand(request.Name);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).RequireAuthorization();
        }
    }
}