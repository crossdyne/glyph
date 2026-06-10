using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Projects;

namespace Glyph.Assets.Domain.Models
{
    public sealed class AssetProjects 
    {
        public AssetId AssetId { get; private set; }
        public ProjectId ProjectId { get; private set; }

        private AssetProjects()
        {
            
        }

        private AssetProjects(AssetId assetId, ProjectId projectId)
        {
            AssetId = assetId;
            ProjectId = projectId;
        }

        internal static AssetProjects Create(AssetId assetId, ProjectId projectId) => new(assetId, projectId);
    }
}