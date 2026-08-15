using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetGlobalMetadata
{
    public sealed record GetGlobalMetadataQuery() : IRequest<List<AssetMetadataResponse>>;
}