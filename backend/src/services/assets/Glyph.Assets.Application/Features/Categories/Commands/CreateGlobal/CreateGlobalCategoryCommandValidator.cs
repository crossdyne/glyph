using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreateGlobal
{
    public class CreateGlobalCategoryCommandValidator : AbstractValidator<CreateGlobalCategoryCommand>
    {
        public CreateGlobalCategoryCommandValidator()
        {
            Include(new CategoryNameValidator());
        }
    }
}