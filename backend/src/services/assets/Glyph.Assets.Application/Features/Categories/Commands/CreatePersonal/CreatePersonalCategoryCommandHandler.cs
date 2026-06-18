using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Categories;
using Glyph.Assets.Domain.ValueObjects.Shared;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreatePersonal
{
    public sealed class CreatePersonalCategoryCommandHandler(
        ICategoryRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreatePersonalCategoryCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreatePersonalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = Category.Create(CategoryName.Create(request.Name), UserId.From(request.UserId));

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