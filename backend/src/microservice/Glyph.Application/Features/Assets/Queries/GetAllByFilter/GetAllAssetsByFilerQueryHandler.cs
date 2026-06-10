using Glyph.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Application.Features.Assets.Queries.GetAllByFilter
{
    public sealed class GetAllAssetsByFilerQueryHandler(IAssetRepository repository) : IRequestHandler<GetAllAssetsByFilerQuery, List<AssetResponse>>
    {
        public async Task<List<AssetResponse>> Handle(GetAllAssetsByFilerQuery request, CancellationToken cancellationToken)
            => await repository.GetAllByFiler(request.ProjectId, request.UserId);
    }
}