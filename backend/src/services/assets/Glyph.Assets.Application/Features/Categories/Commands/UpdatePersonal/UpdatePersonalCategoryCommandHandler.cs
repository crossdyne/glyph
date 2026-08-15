using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<UpdatePersonalCategoryCommand, Result>
    {
        public async Task<Result> Handle(UpdatePersonalCategoryCommand request, CancellationToken cancellationToken)
        {
            Maybe<Category> maybe = await repository.GetByAsync(x => x.Id == request.CategoryId && x.UserId == request.UserId, cancellationToken);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Данной категории не существует."));

            Category category = maybe.Value;
            category.UpdateName(CategoryName.Create(request.Name));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}