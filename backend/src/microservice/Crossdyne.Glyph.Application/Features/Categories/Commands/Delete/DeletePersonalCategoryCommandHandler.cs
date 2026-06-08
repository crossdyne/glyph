using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.Delete
{
    public sealed class DeletePersonalCategoryCommandHandler(
        ICategoryRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<DeletePersonalCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeletePersonalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Maybe<Category> maybe = await repository.GetByAsync(c => c.Id == request.CategoryId && c.UserId == request.UserId, cancellationToken);

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