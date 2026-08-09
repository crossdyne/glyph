using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Domain.ValueObjects.Shared;

namespace Glyph.Assets.Tests.Unit
{
    public class DomainModelTests
    {
        private static readonly AssetName assetName =  AssetName.Create("Valid Name");
        private static readonly S3Key s3Key = S3Key.Create("Avatar", ["users", "profile-avatar"], "avatar.svg"); 
        private static readonly AssetType assetType = AssetType.Svg;
        private static readonly Format format = Format.Svg;
        private static readonly MimeType mimeType = MimeType.Svg;
        private static readonly SizeBytes sizeBytes = SizeBytes.Create(12567);
        private static readonly CategoryId categoryId = CategoryId.From(Guid.CreateVersion7());
        private static readonly IReadOnlyCollection<ProjectId> projectIds = [ProjectId.From(Guid.NewGuid()), ProjectId.From(Guid.NewGuid()), ProjectId.From(Guid.NewGuid())];
        private static readonly UserId userId = UserId.From(Guid.NewGuid());

        #region Asset - Create

        [Fact]
        public void Asset_Create_FullValidValue_ReturnAsset()
        {
            Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, [.. projectIds], userId);

            Assert.NotNull(asset);
            Assert.Equal(s3Key, asset.S3Key);
            Assert.Equal(assetType, asset.AssetType);
            Assert.Equal(format, asset.Format);
            Assert.Equal(mimeType, asset.MimeType);
            Assert.Equal(sizeBytes, asset.SizeBytes);
            Assert.Equal(categoryId, asset.CategoryId);
            Assert.Equal(projectIds.Count, asset.AssetProjects.Count);
            Assert.Equal(projectIds.First().Value, asset.AssetProjects.First().ProjectId);
            Assert.NotNull(asset.UserId);
        }

        [Fact]
        public void Asset_Create_ValidValueWithOutProjects_ReturnAsset()
        {
            Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, null, userId);

            Assert.Empty(asset.AssetProjects);
        }

        [Fact]
        public void Asset_Create_ValidValueWithOutUserId_ReturnAsset()
        {
            Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, [.. projectIds], null);

            Assert.Null(asset.UserId);
        }

        #endregion

        #region Asset - AttachToProject

        [Fact]
        public void AttachToProject_ValidValue_CountProjectIdsChanges()
        {
            Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, [.. projectIds], userId);

            Guid guid = Guid.NewGuid();
            ProjectId projectId = ProjectId.From(guid);
            asset.AttachToProject(projectId);

            Assert.Equal(projectIds.Count + 1, asset.AssetProjects.Count);
        }

        [Fact]
        public void AttachToProject_ExistValue_CountProjectIdsNotChanges()
        {
            Asset asset = Asset.Create(assetName, s3Key, assetType, format, mimeType, sizeBytes, categoryId, [.. projectIds], userId);

            ProjectId projectId = projectIds.First();
            asset.AttachToProject(projectId);

            Assert.Equal(projectIds.Count, asset.AssetProjects.Count);
        }

        #endregion

        #region Category - Create

        [Fact]
        public void Create_WithNullUserId_ShouldBePublic()
        {
            var category = Category.Create(CategoryName.Create("Icons"), userId: null);

            Assert.True(category.IsPublic);
        }

        [Fact]
        public void Create_WithUserId_ShouldBePrivate()
        {
            var userId = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("Icons"), userId);

            Assert.False(category.IsPublic);
        }

        #endregion

        #region Category - CanAccess

        [Fact]
        public void CanAccess_WhenPublic_ReturnTrueForAnyUser()
        {
            var category = Category.Create(CategoryName.Create("Fonts"), userId: null);

            Assert.True(category.CanAccess(UserId.From(Guid.NewGuid())));
        }

        [Fact]
        public void CanAccess_WhenPrivateAndWrongUser_ReturnFalse()
        {
            var owner = UserId.From(Guid.NewGuid());
            var other = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("Fonts"), owner);
            
            Assert.False(category.CanAccess(other));
            Assert.True(category.CanAccess(owner));
        }

        #endregion
    }
}