using Glyph.Bff.Contracts.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Assets.Command.CreatePersonal
{
    public static class CreatePersonalAssetEndpoint
    {
        public static void MapCreatePersonalAsset(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/personal/asset", async (
                [FromForm] CreateAssetBffRequest request, 
                [FromServices] IMediator mediator) =>
            {
                await using var fileStream = request.File.OpenReadStream();

                var command = new CreatePersonalAssetCommand(fileStream, request.File.FileName, request.CategoryId, request.ProjectIdsJson);
                var result = await mediator.Send(command);

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).DisableAntiforgery().RequireAuthorization();   
        }
    }
}