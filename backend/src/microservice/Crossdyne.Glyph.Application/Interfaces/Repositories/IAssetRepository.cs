using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Domain.ValueObjects.Projects;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Interfaces.Repositories
{
    public interface IAssetRepository : IRepository<Asset>
    {
        Task<List<AssetResponse>> GetAllByFiler( Guid projectId, Guid? userId);
        Task<bool> HasProjectsLinksAsync(ProjectId projectId, CancellationToken cl);
    }
}