using System.Net;
using FileService.Client;
using FluentAssertions;
using Glyph.Assets.Application.Features.Account.EventHandlers;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Shared.Contracts.FileService.Interfaces;
using Shared.Contracts.Messaging.Events;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace Glyph.Assets.Tests.Integration.Handlers.Events
{
    public class UserAccountDeletedIntegrationEventHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UserAccountDeletedIntegrationEventHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UserAccountDeletedIntegrationEventHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var assetRepo = new AssetRepository(_fixture.DbContext);
            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);
            var fileClient = new FileStorageClient(new HttpClient{ BaseAddress = new Uri(_fixture.FileServiceMock.Url!) });
            var logger = NullLogger<UserAccountDeletedIntegrationEventHandler>.Instance;
            
            _handler = new UserAccountDeletedIntegrationEventHandler(assetRepo, categoryRepo, fileClient, unitOfWork, logger);
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_DeletesData()
        {
            _fixture.FileServiceMock.Reset();

            var userId = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("Category"), userId);

            var asset = Asset.Create(
                AssetName.Create("Valid Name"), 
                S3Key.Create("Avatar", ["users", "profile-avatar"], "avatar.svg"), 
                AssetType.Svg, 
                Format.Svg, 
                MimeType.Svg, 
                SizeBytes.Create(12567), 
                category.Id, 
                [], userId);

            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            _fixture.FileServiceMock
                .Given(Request.Create().WithPath("/**").UsingDelete())
                .RespondWith(Response.Create().WithStatusCode(HttpStatusCode.NoContent));

            var @event = new UserAccountDeletedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, userId);
            await _handler.HandleAsync(@event, _cancellationToken);

            var categories = await _fixture.DbContext.Set<Category>().Where(c => c.UserId == userId).ToListAsync(_cancellationToken);
            var assets = await _fixture.DbContext.Set<Asset>().Where(a => a.UserId == userId).ToListAsync(_cancellationToken);

            categories.Should().BeEmpty();
            assets.Should().BeEmpty();
        }

        [Fact]
        public async Task HandleAsync_ValidEvent_FailureFileService_ThrowException()
        {
            var userId = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("Category"), userId);
            var asset = Asset.Create(
                AssetName.Create("Valid Name"), 
                S3Key.Create("Avatar", ["users", "profile-avatar"], "avatar.svg"), 
                AssetType.Svg, 
                Format.Svg, 
                MimeType.Svg, 
                SizeBytes.Create(12567), 
                category.Id, 
                [], 
                userId);

            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var fileClientMock = new Mock<IFileServiceClient>();

            fileClientMock
                .Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new HttpRequestException("S3 is down"));

            var handler = new UserAccountDeletedIntegrationEventHandler(
                new AssetRepository(_fixture.DbContext),
                new CategoryRepository(_fixture.DbContext),
                fileClientMock.Object,
                new UnitOfWork(_fixture.DbContext),
                NullLogger<UserAccountDeletedIntegrationEventHandler>.Instance);

            var @event = new UserAccountDeletedIntegrationEvent(Guid.NewGuid(), DateTime.UtcNow, userId);
            await Assert.ThrowsAnyAsync<Exception>(async () => await handler.HandleAsync(@event, _cancellationToken));

            var categories = await _fixture.DbContext.Set<Category>().Where(c => c.UserId == userId).ToListAsync(_cancellationToken);
            var assets = await _fixture.DbContext.Set<Asset>().Where(a => a.UserId == userId).ToListAsync(_cancellationToken);

            categories.Should().HaveCount(1);
            assets.Should().HaveCount(1);
        }
    }
}