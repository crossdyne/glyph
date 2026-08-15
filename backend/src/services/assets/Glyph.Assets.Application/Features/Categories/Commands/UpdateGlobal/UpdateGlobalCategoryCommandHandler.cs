using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal
{
    public sealed class UpdateGlobalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdateGlobalCategoryCommand request, CancellationToken cancellationToken)
        {
            Maybe<Category> maybe = await repository.GetByAsync(x => x.Id == request.CategoryId, cancellationToken);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Данной категории не существует."));

            Category category = maybe.Value;
            category.UpdateName(CategoryName.Create(request.Name));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}