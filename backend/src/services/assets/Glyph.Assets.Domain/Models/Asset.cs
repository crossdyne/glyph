using Glyph.Assets.Domain.Primitives;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Domain.ValueObjects.Shared;

namespace Glyph.Assets.Domain.Models
{
    public sealed class Asset : AggregateRoot<AssetId>
    {
        public AssetName AssetName { get; private set; }
        public S3Key S3Key { get; private set; }
        public AssetType AssetType { get; private set; }
        public Format Format { get; private set; }
        public MimeType MimeType { get; private set; }
        public SizeBytes SizeBytes { get; private set; }
        public bool IsPublic { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdateAt { get; private set; }

        public UserId? UserId { get; private set; }
        public CategoryId CategoryId { get; private set; }

        private List<AssetProjects> _assetProjects = [];
        public IReadOnlyCollection<AssetProjects> AssetProjects => _assetProjects.AsReadOnly();

        private Asset()
        {
            
        }

        private Asset(AssetName assetName, S3Key s3Key, AssetType assetType, Format format, MimeType mimeType, SizeBytes sizeBytes, CategoryId categoryId, List<ProjectId>? projectIds, UserId? userId) : base(AssetId.New())
        {
            if (projectIds is not null)
            {
                foreach (var projectId in projectIds)
                {
                    AttachToProject(projectId);
                }   
            }

            AssetName = assetName;
            S3Key = s3Key;
            AssetType = assetType;
            Format = format;
            MimeType = mimeType;
            SizeBytes = sizeBytes;

            CategoryId = categoryId;

            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;

            UserId = userId;
            IsPublic = UserId == null;
        }

        public static Asset Create(AssetName assetName, S3Key s3Key, AssetType assetType, Format format, MimeType mimeType, SizeBytes sizeBytes, CategoryId categoryId, List<ProjectId>? projectIds = null, UserId? userId = null)
        {
            return new(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, projectIds, userId);
        }

        public void AttachToProject(ProjectId projectId)
        {
            if (_assetProjects.Any(ap => ap.ProjectId == projectId)) 
                return;

            var link = Models.AssetProjects.Create(Id, projectId);
            _assetProjects.Add(link);
            UpdateAt = DateTime.UtcNow;
        }

        public void UpdateContent(AssetName assetName, S3Key s3Key, Format format, MimeType mimeType, AssetType assetType, SizeBytes sizeBytes)
        {
            AssetName = assetName;
            S3Key = s3Key;
            Format = format;
            MimeType = mimeType;
            AssetType = assetType;
            SizeBytes = sizeBytes;
        }
    }
}