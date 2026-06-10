using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.CreateGlobal
{
    public class CreateGlobalCategoryCommandValidator : AbstractValidator<CreateGlobalCategoryCommand>
    {
        public CreateGlobalCategoryCommandValidator()
        {
            Include(new CategoryNameValidator());
        }
    }
}