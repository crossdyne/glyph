using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal
{
    public sealed class UpdateGlobalCategoryCommandValidator : AbstractValidator<UpdateGlobalCategoryCommand>
    {
        public UpdateGlobalCategoryCommandValidator()
        {
            Include(new CategoryIdValidator());
            Include(new CategoryNameValidator());
        }
    }
}