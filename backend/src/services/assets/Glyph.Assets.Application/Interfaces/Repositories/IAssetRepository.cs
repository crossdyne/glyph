using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Interfaces.Repositories
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<List<AssetResponse>> GetAllByFiler( Guid projectId, Guid? userId);
        Task<List<AssetMetadataResponse>> GetMetadata(Guid userId);
        Task<bool> HasProjectsLinksAsync(ProjectId projectId, CancellationToken cl);
    }
}