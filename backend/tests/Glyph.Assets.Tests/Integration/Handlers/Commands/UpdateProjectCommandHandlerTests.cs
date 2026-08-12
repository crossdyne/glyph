using FluentAssertions;
using Glyph.Assets.Application.Features.Projects.Commands.Update;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class UpdateProjectCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdateProjectCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdateProjectCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;
            var repo = new ProjectRepository(_fixture.DbContext);
            var uow = new UnitOfWork(_fixture.DbContext);
            _handler = new UpdateProjectCommandHandler(repo, uow);
        }

        [Fact]
        public async Task Handle_ProjectNotFound_ReturnsNotFoundError()
        {
            var command = new UpdateProjectCommand(
                ProjectId: ProjectId.New(),
                Name: "NewName");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ValidCommand_UpdatesProjectName()
        {
            var project = Project.Create(ProjectName.Create("Old"), ProjectCode.Create("OLD"));
            await _fixture.DbContext.Set<Project>().AddAsync(project, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new UpdateProjectCommand(
                ProjectId: project.Id,
                Name: "Updated");

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsSuccess.Should().BeTrue();

            var updated = await _fixture.DbContext.Set<Project>().FirstOrDefaultAsync(p => p.Id == project.Id, _cancellationToken);
            updated?.Name.Value.Should().Be("Updated");
        }
    }
}