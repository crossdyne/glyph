using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetAggregated
{
    public sealed class GetAggregatedAssetsQueryHandler(IAssetRepository assetRepository, IProjectRepository projectRepository) : IRequestHandler<GetAggregatedAssetsQuery, Result<List<AssetMetadataResponse>>>
    {
        public async Task<Result<List<AssetMetadataResponse>>> Handle(GetAggregatedAssetsQuery request, CancellationToken cancellationToken)
        {
            Maybe<Project> maybeProject = await projectRepository.GetProjectByCode(request.ProjectCode);

            if (maybeProject.IsNone)
                return Result<List<AssetMetadataResponse>>.Failure(new Error(ErrorCode.NotFound, "Для данного проекта нету никаких иконок"));

            var assets = await assetRepository.GetAggregated(maybeProject.Value.Id, request.UserId);

            return Result<List<AssetMetadataResponse>>.Success(assets);
        }
    }
}