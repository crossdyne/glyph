using Crossdyne.Toolkit.Results;
using FileService.Client;
using FluentAssertions;
using Glyph.Assets.Application.Features.Assets.Commands.UpdateGlobal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands;

public class UpdateGlobalAssetCommandHandlerTests : IClassFixture<TestFixture>
{
    private readonly ITestOutputHelper _output;
    private readonly TestFixture _fixture;
    private readonly UpdateGlobalAssetCommandHandler _handler;
    private readonly CancellationToken _cancellationToken = default;

    public UpdateGlobalAssetCommandHandlerTests(TestFixture fixture, ITestOutputHelper output)
    {
        _output = output;
        _fixture = fixture;

        var assetRepo = new AssetRepository(_fixture.DbContext);
        var unitOfWork = new UnitOfWork(_fixture.DbContext);
        var metadataDetector = new FakeMetadataDetector();
        var fileClient = new FileStorageClient(new HttpClient
        {
            BaseAddress = new Uri(_fixture.FileServiceMock.Url!)
        });

        _handler = new UpdateGlobalAssetCommandHandler(assetRepo, unitOfWork, metadataDetector, fileClient);
    }

    [Fact]
    public async Task Handle_ValidCommandWithFile_UpdatesAssetAndUploadsNewFile()
    {
        var category = Category.Create(CategoryName.Create("Cat"), userId: null);
        await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

        var asset = Asset.Create(
            AssetName.Create("old-name.svg"),
            S3Key.Create("Avatars", ["user"], "old-name.svg"),
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
            .Given(Request.Create().WithPath("/**").UsingPost())
            .RespondWith(Response.Create().WithStatusCode(200));

        _fixture.FileServiceMock
            .Given(Request.Create().WithPath("/**").UsingDelete())
            .RespondWith(Response.Create().WithStatusCode(200));

        var command = new UpdateGlobalAssetCommand(
            FileContent: new MemoryStream([1, 2, 3]),
            SizeBytes: 3,
            FileName: "new-name.svg",
            AssetName: "UpdatedName",
            CategoryId: category.Id.Value.ToString(),
            AssetId: asset.Id);

        Result result = await _handler.Handle(command, _cancellationToken);

        result.IsSuccess.Should().BeTrue();

        var updated = await _fixture.DbContext.Set<Asset>()
            .FirstOrDefaultAsync(a => a.Id == asset.Id, _cancellationToken);

        updated.Should().NotBeNull();
        updated!.AssetName.Value.Should().Be("UpdatedName");
        Guid.TryParse(updated.S3Key.FileName.Split('.').First(), out var _).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_AssetNotFound_ReturnsNotFoundError()
    {
        var command = new UpdateGlobalAssetCommand(
            FileContent: null,
            SizeBytes: null,
            FileName: null,
            AssetName: "Whatever",
            CategoryId: Guid.NewGuid().ToString(),
            AssetId: AssetId.From(Guid.NewGuid()));

        Result result = await _handler.Handle(command, _cancellationToken);

        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_UploadFails_ReturnsErrorAndDoesNotUpdateAsset()
    {
        var category = Category.Create(CategoryName.Create("Cat"), userId: null);
        await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

        var asset = Asset.Create(
            AssetName.Create("original.svg"),
            S3Key.Create("Avatars", ["user"], "original.svg"),
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
            .Given(Request.Create().WithPath("/**").UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(500)
                .WithBody("""{ "errors": ["S3 error"] }"""));

        var command = new UpdateGlobalAssetCommand(
            FileContent: new MemoryStream([1, 2, 3]),
            SizeBytes: 3,
            FileName: "new.svg",
            AssetName: "ShouldNotSave",
            CategoryId: category.Id.Value.ToString(),
            AssetId: asset.Id);

        Result result = await _handler.Handle(command, _cancellationToken);

        result.IsFailure.Should().BeTrue();

        var assetInDb = await _fixture.DbContext.Set<Asset>()
            .FirstOrDefaultAsync(a => a.Id == asset.Id, _cancellationToken);

        assetInDb.Should().NotBeNull();
        assetInDb.AssetName.Value.Should().Be("original.svg");
        Guid.TryParse(assetInDb.S3Key.FileName.Split('.').First(), out var _).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WithoutFile_UpdatesOnlyNameAndCategory()
    {
        var oldCategory = Category.Create(CategoryName.Create("OldCat"), userId: null);
        var newCategory = Category.Create(CategoryName.Create("NewCat"), userId: null);
        await _fixture.DbContext.Set<Category>().AddRangeAsync(oldCategory, newCategory);
        await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

        var asset = Asset.Create(
            AssetName.Create("old-name.svg"),
            S3Key.Create("Avatars", ["user"], "old-name.svg"),
            AssetType.Svg,
            Format.Svg,
            MimeType.Svg,
            SizeBytes.Create(100),
            oldCategory.Id,
            projectIds: null,
            userId: null);

        await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);
        await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

        var command = new UpdateGlobalAssetCommand(
            FileContent: null,
            SizeBytes: null,
            FileName: null,
            AssetName: "OnlyNameChanged",
            CategoryId: newCategory.Id.Value.ToString(),
            AssetId: asset.Id);

        Result result = await _handler.Handle(command, _cancellationToken);

        result.IsSuccess.Should().BeTrue();

        var updated = await _fixture.DbContext.Set<Asset>()
            .FirstOrDefaultAsync(a => a.Id == asset.Id, _cancellationToken);

        updated.Should().NotBeNull();
        updated!.AssetName.Value.Should().Be("OnlyNameChanged");
        updated.CategoryId.Value.Should().Be(newCategory.Id.Value);
        Guid.TryParse(updated.S3Key.FileName.Split('.').First(), out var _).Should().BeTrue();
    }
}