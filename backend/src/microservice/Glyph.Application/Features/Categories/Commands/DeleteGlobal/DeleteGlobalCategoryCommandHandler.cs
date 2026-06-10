using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Application.Interfaces;
using Glyph.Application.Interfaces.Repositories;
using Glyph.Domain.Models;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteGlobalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Maybe<Category> maybe = await repository.GetByAsync(c => c.Id == request.CategoryId, cancellationToken);

                if (maybe.IsNone)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной категории не существует."));

                Category category = maybe.Value;

                repository.Remove(category);

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Delete, $"Произошел непредвиденный сбой при удаление: {ex}"));
            }
        }
    }
}