using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetMetadata
{
    public sealed record GetAssetsMetadataQuery(Guid UserId) : IRequest<List<AssetMetadataResponse>>;
}