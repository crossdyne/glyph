using FluentAssertions;
using Glyph.Assets.Application.Features.Projects.Commands.Delete;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Assets;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class DeleteProjectCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly DeleteProjectCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public DeleteProjectCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;
            var projectRepo = new ProjectRepository(_fixture.DbContext);
            var assetRepo = new AssetRepository(_fixture.DbContext);
            var uow = new UnitOfWork(_fixture.DbContext);
            _handler = new DeleteProjectCommandHandler(projectRepo, assetRepo, uow);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsNotFoundError()
        {
            var command = new DeleteProjectCommand(ProjectId.From(Guid.NewGuid()));
            var result = await _handler.Handle(command, CancellationToken.None);
            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ProjectHasLinks_ReturnsValidationError()
        {
            var category = Category.Create(CategoryName.Create("Cat"), userId: null);
            var project = Project.Create(ProjectName.Create("Linked"), ProjectCode.Create("LINK"));
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.Set<Project>().AddAsync(project, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            // Создаём asset, привязанный к проекту
            var asset = Asset.Create(
                AssetName.Create("a.svg"),
                S3Key.Create("b", ["f"], "a.svg"),
                AssetType.Svg, Format.Svg, MimeType.Svg, SizeBytes.Create(1),
                category.Id, projectIds: [project.Id], userId: null);
            await _fixture.DbContext.Set<Asset>().AddAsync(asset, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new DeleteProjectCommand(project.Id);
            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ValidCommand_DeletesProject()
        {
            var project = Project.Create(ProjectName.Create("ToDelete"), ProjectCode.Create("DEL"));
            await _fixture.DbContext.Set<Project>().AddAsync(project, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new DeleteProjectCommand(project.Id);
            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            var inDb = await _fixture.DbContext.Set<Project>().FirstOrDefaultAsync(p => p.Id == project.Id, _cancellationToken);
            inDb.Should().BeNull();
        }
    }
}