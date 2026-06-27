using Glyph.Bff.Interfaces.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Glyph.Bff.Features.Categories
{
    public static class AggregatedCategoryEndpoints
    {
        public static void Map(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/v1/personal/category/all", async ([FromServices] IPersonalCategoriesClient client)     
                => Results.Ok(await client.GetAllPersonalAndGlobal())).RequireAuthorization();
        }
    }
}