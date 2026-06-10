using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Categories.Commands.UpdateGlobal
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