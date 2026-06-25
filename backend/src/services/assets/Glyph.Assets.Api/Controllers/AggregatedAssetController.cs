using Crossdyne.Toolkit.Results;
using Glyph.Assets.Api.Extensions;
using Glyph.Assets.Application.Features.Assets.Queries.GetAggregated;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Api.Controllers
{
    [Route("api/v1/aggregated/asset")]
    [Authorize]
    public sealed class AggregatedAssetController(IMediator mediator) : Controller
    {   
        [HttpGet("{projectCode}")]
        public async Task<IActionResult> GetAggregatedAssets([FromRoute] string projectCode)
        {
            var extractResult = this.ExtractCredentials(User);

            if (extractResult.IsFailure)
                return extractResult.Value.Result;

            var query = new GetAggregatedAssetsQuery(extractResult.Value.UserId, projectCode);
            Result<List<AssetMetadataResponse>> result = await mediator.Send(query);

            if (result.IsFailure)
                return this.MapActionResult(result.Errors);

            return Ok(result.Value);
        }
    }
}