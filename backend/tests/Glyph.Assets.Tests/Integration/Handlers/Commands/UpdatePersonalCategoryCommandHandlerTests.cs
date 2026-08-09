using Crossdyne.Toolkit.Results;
using FluentAssertions;
using Glyph.Assets.Application.Features.Categories.Commands.UpdatePersonal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class UpdatePersonalCategoryCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly UpdatePersonalCategoryCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public UpdatePersonalCategoryCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new UpdatePersonalCategoryCommandHandler(categoryRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_UpdateCategory()
        {
            var userId = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("TestCategory"), userId: userId);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new UpdatePersonalCategoryCommand(category.Id, userId, "UpdateCatName");
            Result result = await _handler.Handle(command, _cancellationToken);

            Category? catInDb = await _fixture.DbContext.Set<Category>().FirstOrDefaultAsync(c => c.Id == category.Id, _cancellationToken);
            catInDb.Should().NotBeNull();
            catInDb.Name.Value.Should().Be("UpdateCatName");
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnNotFound()
        {
            var command = new UpdatePersonalCategoryCommand(Guid.NewGuid(), Guid.NewGuid(), "UpdateCatName");
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeFalse();
            Error? error = result.Errors.FirstOrDefault(x => x.Code == ErrorCode.NotFound);
            error?.Code.Should().Be(ErrorCode.NotFound);
        }
        
    }
}