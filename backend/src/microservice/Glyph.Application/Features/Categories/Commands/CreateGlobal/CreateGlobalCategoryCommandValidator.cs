using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Categories.Commands.CreateGlobal
{
    public class CreateGlobalCategoryCommandValidator : AbstractValidator<CreateGlobalCategoryCommand>
    {
        public CreateGlobalCategoryCommandValidator()
        {
            Include(new CategoryNameValidator());
        }
    }
}