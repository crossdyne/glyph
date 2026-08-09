using Crossdyne.Toolkit.Results;
using FluentAssertions;
using Glyph.Assets.Application.Features.Categories.Commands.DeletePersonal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class DeletePersonalCategoryCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly DeletePersonalCategoryCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public DeletePersonalCategoryCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new DeletePersonalCategoryCommandHandler(categoryRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_DeletesCategory()
        {
            var userId = UserId.From(Guid.NewGuid());
            var category = Category.Create(CategoryName.Create("TestCategory"), userId: userId);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new DeletePersonalCategoryCommand(category.Id, userId.Value);
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            var categoryInDb = await _fixture.DbContext.Set<Category>().FirstOrDefaultAsync(c => c.Name == category.Name && c.UserId == userId, _cancellationToken);
            categoryInDb.Should().BeNull();
        }
        
        [Fact]
        public async Task Handle_ValidCommand_ReturnNotFoundError()
        {
            var command = new DeletePersonalCategoryCommand(Guid.NewGuid(), Guid.NewGuid());
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeFalse();
            Error? error = result.Errors.FirstOrDefault(x => x.Code == ErrorCode.NotFound);
            error?.Code.Should().Be(ErrorCode.NotFound);
        }    
        
    }
}