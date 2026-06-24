using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreateGlobal
{
    public sealed class CreateGlobalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateGlobalCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateGlobalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = Category.Create(CategoryName.Create(request.Name));

                await repository.AddAsync(category, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result<string>.Success(category.Id.ToString());
            }
            catch (Exception ex)
            {
                return Result<string>.Failure(new Error(ErrorCode.Save, $"Произошла ошибка сохранения: {ex}"));
            }
        }
    }
}