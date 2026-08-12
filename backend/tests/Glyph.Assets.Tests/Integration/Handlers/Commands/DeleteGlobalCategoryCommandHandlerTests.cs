using Crossdyne.Toolkit.Results;
using FluentAssertions;
using Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Infrastructure.Persistence;
using Glyph.Assets.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Glyph.Assets.Tests.Integration.Handlers.Commands
{
    public class DeleteGlobalCategoryCommandHandlerTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
        private readonly DeleteGlobalCategoryCommandHandler _handler;
        private readonly CancellationToken _cancellationToken = default;

        public DeleteGlobalCategoryCommandHandlerTests(TestFixture fixture)
        {
            _fixture = fixture;

            var categoryRepo = new CategoryRepository(_fixture.DbContext);
            var unitOfWork = new UnitOfWork(_fixture.DbContext);

            _handler = new DeleteGlobalCategoryCommandHandler(categoryRepo, unitOfWork);
        }
        
        [Fact]
        public async Task Handle_ValidCommand_DeletesCategory()
        {
            var category = Category.Create(CategoryName.Create("TestCategory"), userId: null);
            await _fixture.DbContext.Set<Category>().AddAsync(category, _cancellationToken);
            await _fixture.DbContext.SaveChangesAsync(_cancellationToken);

            var command = new DeleteGlobalCategoryCommand(category.Id);
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeTrue();
            var categoryInDb = await _fixture.DbContext.Set<Category>().FirstOrDefaultAsync(c => c.Name == category.Name, _cancellationToken);
            categoryInDb.Should().BeNull();
        }
        
        [Fact]
        public async Task Handle_ValidCommand_ReturnNotFoundError()
        {
            var command = new DeleteGlobalCategoryCommand(Guid.NewGuid());
            Result result = await _handler.Handle(command, _cancellationToken);

            result.IsSuccess.Should().BeFalse();
            Error? error = result.Errors.FirstOrDefault(x => x.Code == ErrorCode.NotFound);
            error?.Code.Should().Be(ErrorCode.NotFound);
        }    
    }
}