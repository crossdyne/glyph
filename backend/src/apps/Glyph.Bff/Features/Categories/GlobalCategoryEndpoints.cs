using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories
{
    public static class GlobalCategoryEndpoints
    {
        public static void MapGlobalCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/global/category", async (
                [FromBody] CreateCategoryRequest request,
                [FromServices] IGlobalCategoriesClient client) =>   
            {
                var result = await client.AddWithResultAsync<string, CreateCategoryRequest>(new CreateCategoryRequest(request.Name)).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).RequireAuthorization();

            app.MapDelete("api/v1/global/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromServices] IGlobalCategoriesClient client) =>
            {
                var result = await client.DeleteAsync(categoryId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();

             app.MapPatch("api/v1/global/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromBody] UpdateCategoryRequest request,
                [FromServices] IGlobalCategoriesClient client) =>
            {
                var result = await client.UpdateAsync(categoryId, new UpdateCategoryRequest(request.Name)).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).RequireAuthorization();

            app.MapGet("/api/v1/global/category", async ([FromServices] IGlobalCategoriesClient client) =>
            {
                var result = await client.GetAllAsync();

                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}