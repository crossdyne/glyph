using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<List<AssetMetadataResponse>> GetAggregated( Guid projectId, Guid userId);
        Task<List<AssetMetadataResponse>> GetMetadata(Guid? userId);
        Task<bool> HasProjectsLinksAsync(ProjectId projectId, CancellationToken cl);
    }
}