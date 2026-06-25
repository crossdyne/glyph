using Glyph.Assets.Api.Extensions;
using Glyph.Assets.Application.Features.Categories.Queries.GetAggregated;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Api.Controllers
{
    [Route("api/v1/aggregated/category")]
    [Authorize]
    public class AggregatedCategoryController(IMediator mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetAggregatedCategories()
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetAggregatedCategoriesQuery(extractResult.Value.UserId);
            List<CategoryResponse> result = await mediator.Send(query);

            return Ok(result);
        }
    }
}