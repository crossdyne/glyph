using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetMetadata
{
    public sealed record GetAssetsMetadataQuery(Guid UserId) : IRequest<List<AssetMetadataResponse>>;
}