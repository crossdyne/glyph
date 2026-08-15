using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetGlobalMetadata
{
    public sealed class GetGlobalMetadataQueryHandler(IAssetRepository repository) : IRequestHandler<GetGlobalMetadataQuery, List<AssetMetadataResponse>>
    {
        public async Task<List<AssetMetadataResponse>> Handle(GetGlobalMetadataQuery request, CancellationToken cancellationToken)
            => await repository.GetMetadata(userId: null);
    }
}