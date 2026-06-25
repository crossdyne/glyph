using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetAggregated
{
    public sealed record GetAggregatedAssetsQuery(Guid UserId, string ProjectCode) : IRequest<Result<List<AssetMetadataResponse>>>;
}