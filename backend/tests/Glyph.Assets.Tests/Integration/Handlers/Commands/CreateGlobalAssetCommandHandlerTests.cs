using Crossdyne.Toolkit.Results;
using FileService.Client;
using FluentAssertions;
using Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class CreateGlobalAssetCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly CreateGlobalAssetCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public CreateGlobalAssetCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var assetRepo = new AssetRepository(_fixture.DbContext);
            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            var metadataDetector = new FakeMetadataDetector();
            var fileClient = new FileStorageClient(new HttpClient{ BaseAddress = new Uri(_fixture.FileServiceMock.Url!) });

            _handler = new CreateGlobalAssetCommandHandler(assetRepo, categoryRepo, unitOfWork, metadataDetector, fileClient);
        }

        [Fact]
        public async Task Handle_ValidCommand_CreatesAssetAndUploadsFile()
        {
            _fixture.FileServiceMock.Reset();
            
            var category = Category.Create(CategoryName.Create("ValidCat"), userId: null);
            _fixture.DbContext.Set<Category>().Add(category);
            
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            _fixture.FileServiceMock
                .Given(Request.Create().WithPath("/**").UsingPost())
                .RespondWith(Response.Create().WithStatusCode(200));

            var command = new CreateGlobalAssetCommand(
                AssetName: "avatar.svg",
                CategoryId: category.Id,
                FileContent: new MemoryStream([1, 2, 3]),
                FileName: "avatar.svg",
                SizeBytes: 3,
                Bucket: "assets",
                Folders: ["users"],
                ProjectIds: []);

            Result<string> result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();
            Guid.Parse(result.Value).Should().NotBeEmpty();

            var assetInDb = await _fixture.DbContext.Set<Asset>().FirstOrDefaultAsync(_cancellationToken);
            assetInDb.Should().NotBeNull();
            assetInDb.AssetName.Value.Should().Be("avatar.svg");
        }

        [Fact]
        public async Task Handle_UploadFails_RollsBackAssetFromDatabase()
        {
            _fixture.FileServiceMock.Reset();

            var category = Category.Create(CategoryName.Create("GlobalCat"), userId: null);
            _fixture.DbContext.Set<Category>().Add(category);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            _fixture.FileServiceMock
                .Given(Request.Create().WithPath("/**").UsingPost())
                .RespondWith(Response.Create()
                    .WithStatusCode(500)
                    .WithBody("""{ "errors": ["S3 unavailable"] }"""));

            var command = new CreateGlobalAssetCommand(
                AssetName: "rollback-avatar.svg",
                CategoryId: category.Id.Value,
                FileContent: new MemoryStream([1, 2, 3]),
                FileName: "rollback-avatar.svg",
                SizeBytes: 3,
                Bucket: "assets",
                Folders: ["users"],
                ProjectIds: []);

            Result<string> result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();

            var assetInDb = await _fixture.DbContext.Set<Asset>().FirstOrDefaultAsync(a => a.AssetName == "rollback-avatar.svg", _cancellationToken);
            
            assetInDb.Should().BeNull("при ошибке Upload asset должен быть удалён из БД");
        }

        [Fact]
        public async Task Handle_CategoryIsNotGlobal_ReturnsValidationError()
        {
            _fixture.FileServiceMock.Reset();
            var category = Category.Create(CategoryName.Create("PersonalCat"), userId: null);
            _fixture.DbContext.Set<Category>().Add(category);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new CreateGlobalAssetCommand(
                AssetName: "avatar.svg",
                CategoryId: category.Id.Value,
                FileContent: new MemoryStream(new byte[] { 1, 2, 3 }),
                FileName: "avatar.svg",
                SizeBytes: 3,
                Bucket: "assets",
                Folders: ["users"],
                ProjectIds: []);

            Result<string> result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
        }
    }
}