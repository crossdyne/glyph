using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.UpdateGlobal
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