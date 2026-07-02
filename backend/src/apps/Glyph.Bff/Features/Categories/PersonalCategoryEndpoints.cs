using Glyph.Bff.Extensions;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Requests;
using Shared.Web.Extensions;

namespace Glyph.Bff.Features.Categories
{
    public static class PersonalCategoryEndpoints
    {
        public static void MapPersonalCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("api/v1/personal/category", async (
                [FromBody] CreateCategoryRequest request,
                [FromServices] IPersonalCategoriesClient client) =>
            {                  
                var result = await client.AddWithResultAsync<string, CreateCategoryRequest>(new CreateCategoryRequest(request.Name)).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok(result.Value);
            }).RequireAuthorization();

            app.MapDelete("api/v1/personal/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromServices] IPersonalCategoriesClient client) =>
            {
                var result = await client.DeleteAsync(categoryId).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.NoContent();
            }).RequireAuthorization();

            app.MapPatch("api/v1/personal/category/{categoryId}", async (
                [FromRoute] string categoryId,
                [FromBody] UpdateCategoryRequest request,
                [FromServices] IPersonalCategoriesClient client) =>
            {
                var result = await client.UpdateAsync(categoryId, new UpdateCategoryRequest(request.Name)).ToResult();

                if (result.IsFailure)
                    return result.Errors.MapToMinimalApiResult();

                return Results.Ok();
            }).RequireAuthorization();

            app.MapGet("/api/v1/personal/category", async ([FromServices] IPersonalCategoriesClient client) 
                => Results.Ok(await client.GetAllAsync())).RequireAuthorization();
        }
    }
}