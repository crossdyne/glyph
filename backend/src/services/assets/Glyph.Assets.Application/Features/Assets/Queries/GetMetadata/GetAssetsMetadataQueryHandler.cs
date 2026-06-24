using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetMetadata
{
    public sealed class GetAssetsMetadataQueryHandler(IAssetRepository repository) : IRequestHandler<GetAssetsMetadataQuery, List<AssetMetadataResponse>>
    {
        public async Task<List<AssetMetadataResponse>> Handle(GetAssetsMetadataQuery request, CancellationToken cancellationToken)
            => await repository.GetMetadata(request.UserId);
    }
}