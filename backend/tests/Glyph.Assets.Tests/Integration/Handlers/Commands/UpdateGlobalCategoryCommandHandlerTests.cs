using Crossdyne.Toolkit.Results;
using FluentAssertions;
using Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class UpdateGlobalCategoryCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdateGlobalCategoryCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdateGlobalCategoryCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new UpdateGlobalCategoryCommandHandler(categoryRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_UpdateCategory()
        {
            var category = Category.Create(CategoryName.Create("TestCategory"), userId: null);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new UpdateGlobalCategoryCommand(category.Id, "UpdateCatName");
            Result result = await _handler.Handle(command, _cancellationToken);

            Category? catInDb = await _fixture.DbContext.Set<Category>().FirstOrDefaultAsync(c => c.Id == category.Id, _cancellationToken);
            catInDb.Should().NotBeNull();
            catInDb.Name.Value.Should().Be("UpdateCatName");
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnNotFound()
        {
            var command = new UpdateGlobalCategoryCommand(Guid.NewGuid(), "UpdateCatName");
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeFalse();
            Error? error = result.Errors.FirstOrDefault(x => x.Code == ErrorCode.NotFound);
            error?.Code.Should().Be(ErrorCode.NotFound);
        }
    }
}