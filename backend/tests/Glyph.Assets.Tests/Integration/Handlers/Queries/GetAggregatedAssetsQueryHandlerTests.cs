using Crossdyne.Toolkit.Results;
using FluentAssertions;
using Glyph.Assets.Application.Features.Assets.Queries.GetAggregated;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Tests.Integration.Handlers.Queries
{
    public class GetAggregatedAssetsQueryHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly GetAggregatedAssetsQueryHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public GetAggregatedAssetsQueryHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var assetRepo = new AssetRepository(_fixture.DbContext);
            var projectRepo = new ProjectRepository(_fixture.DbContext);

            _handler = new GetAggregatedAssetsQueryHandler(assetRepo, projectRepo);
        }

        [Fact]
        public async Task Handle_ValidQuery_ReturnGlobalAndPersonalAssets()
        {
            var category = Category.Create(CategoryName.Create("ValidCat"), userId: null);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var project = Project.Create(ProjectName.Create("TestProject"), ProjectCode.Create("TEST_PROJECT"));
            await _fixture.DbContext.Set<Project>().AddAsync(project, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var userId = UserId.From(Guid.NewGuid());
            var personalAsset = Asset.Create(AssetName.Create("avatar.svg"), S3Key.Create("Avatars", ["user", "avatars"], "avatar.svg"), AssetType.Svg, Format.Svg, MimeType.Svg, SizeBytes.Create(100), category.Id, projectIds: [project.Id], userId: userId);
            var globalAsset = Asset.Create(AssetName.Create("icon.svg"), S3Key.Create("Icons", ["storage", "icons"], "icon.svg"), AssetType.Svg, Format.Svg, MimeType.Svg, SizeBytes.Create(100), category.Id, projectIds: [project.Id], userId: null);

            await _fixture.DbContext.Set<Asset>().AddRangeAsync(personalAsset, globalAsset);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var query = new GetAggregatedAssetsQuery(userId, project.Code);
            Result<List<AssetMetadataResponse>> result = await _handler.Handle(query, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            result.Value.Count.Should().Be(2);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsNotFoundError()
        {
            var query = new GetAggregatedAssetsQuery(
                UserId: UserId.From(Guid.NewGuid()), 
                ProjectCode: ProjectCode.Create("NON_EXISTENT"));

            var result = await _handler.Handle(query, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
        }
    }
}