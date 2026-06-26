using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteGlobalCategoryCommand request, CancellationToken cancellationToken)
        {
            Maybe<Category> maybe = await repository.GetByAsync(c => c.Id == request.CategoryId, cancellationToken);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Данной категории не существует."));

            Category category = maybe.Value;

            repository.Remove(category);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}