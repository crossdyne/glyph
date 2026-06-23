using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetAllByFilter
{
    public sealed class GetAllAssetsByFilerQueryHandler(IAssetRepository repository) : IRequestHandler<GetAllAssetsByFilerQuery, List<AssetResponse>>
    {
        public async Task<List<AssetResponse>> Handle(GetAllAssetsByFilerQuery request, CancellationToken cancellationToken)
            => await repository.GetAllByFiler(request.ProjectId, request.UserId);
    }
}