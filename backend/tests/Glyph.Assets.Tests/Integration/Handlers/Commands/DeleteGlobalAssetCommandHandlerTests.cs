using System.Net;
using Crossdyne.Toolkit.Results;
using FileService.Client;
using FluentAssertions;
using Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class DeleteGlobalAssetCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly DeleteGlobalAssetCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public DeleteGlobalAssetCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var assetRepo = new AssetRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            var fileClient = new FileStorageClient(new HttpClient{ BaseAddress = new Uri(_fixture.FileServiceMock.Url!) });

            _handler = new DeleteGlobalAssetCommandHandler(assetRepo, unitOfWork, fileClient);
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesAssetAndDeletesFile()
        {
            var category = Category.Create(CategoryName.Create("ValidCat"), userId: null);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var asset = Asset.Create(
                AssetName.Create("avatar.svg"),
                S3Key.Create("Avatars", ["user", "avatars"], "avatar.svg"),
                AssetType.Svg,
                Format.Svg,
                MimeType.Svg,
                SizeBytes.Create(100),
                category.Id,
                projectIds: null,
                userId: null);

            await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);
            
            _fixture.FileServiceMock
                .Given(Request.Create().WithPath("/**").UsingDelete())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NoContent));

            Guid assetId = asset.Id;

            var command = new DeleteGlobalAssetCommand(assetId);
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            var assetInDb = await _fixture.DbContext.Set<Asset>().FirstOrDefaultAsync(a => a.Id == asset.Id, _cancellationToken);
            assetInDb.Should().BeNull();
        }

        [Fact]
        public async Task Handle_DeletesFails_FileServiceError()
        {            
            var category = Category.Create(CategoryName.Create("ValidCat"), userId: null);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var asset = Asset.Create(
                AssetName.Create("avatar.svg"),
                S3Key.Create("Avatars", ["user", "avatars"], "avatar.svg"),
                AssetType.Svg,
                Format.Svg,
                MimeType.Svg,
                SizeBytes.Create(100),
                category.Id,
                projectIds: null,
                userId: null);

            await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);

            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            _fixture.FileServiceMock
                .Given(Request.Create().WithPath("/**").UsingDelete())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.BadRequest));

            var command = new DeleteGlobalAssetCommand(asset.Id);
            Result result = await _handler.Handle(command, _cancellationToken);

            var assetInDb = await _fixture.DbContext.Set<Asset>().FirstOrDefaultAsync(a => a.Id == asset.Id, _cancellationToken);
            result.IsSuccess.Should().BeFalse();
            assetInDb.Should().BeNull();
        }

        [Fact]
        public async Task Handle_DeletesFails_NotFoundAsset()
        {
            var assetId = Guid.NewGuid();
            var command = new DeleteGlobalAssetCommand(assetId);
            Result result = await _handler.Handle(command, _cancellationToken);

            var assetInDb = await _fixture.DbContext.Set<Asset>().FirstOrDefaultAsync(a => a.Id == assetId, _cancellationToken);
            result.IsSuccess.Should().BeFalse();
            assetInDb.Should().BeNull();
        }
    }
}