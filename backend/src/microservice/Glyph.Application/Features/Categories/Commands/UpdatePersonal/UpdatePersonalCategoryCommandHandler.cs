using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Application.Interfaces;
using Glyph.Application.Interfaces.Repositories;
using Glyph.Domain.Models;
using Glyph.Domain.ValueObjects.Categories;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdatePersonalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Maybe<Category> maybe = await repository.GetByAsync(x => x.Id == request.CategoryId && x.UserId == request.UserId, cancellationToken);

                if (maybe.IsNone)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Данной категории не существует."));

                Category category = maybe.Value;
                category.UpdateName(CategoryName.Create(request.Name));

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Произошел непредвиденный сбой при обновление: {ex}"));
            }
        }
    }
}