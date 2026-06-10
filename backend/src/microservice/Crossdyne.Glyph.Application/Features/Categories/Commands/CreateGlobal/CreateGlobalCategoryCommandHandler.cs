using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Domain.ValueObjects.Categories;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.CreateGlobal
{
    public sealed class CreateGlobalCategoryCommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<CreateGlobalCategoryCommand, Result>
    {
        public async Task<Result> Handle(CreateGlobalCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Category category = Category.Create(CategoryName.Create(request.Name));

                await repository.AddAsync(category, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Save, $"Произошла ошибка сохранения: {ex}"));
            }
        }
    }
}