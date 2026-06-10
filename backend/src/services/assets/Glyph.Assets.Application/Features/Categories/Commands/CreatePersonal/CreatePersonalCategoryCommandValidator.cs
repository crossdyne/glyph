using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreatePersonal
{
    public sealed class CreatePersonalCategoryCommandValidator : AbstractValidator<CreatePersonalCategoryCommand>
    {
        public CreatePersonalCategoryCommandValidator()
        {
            Include(new UserIdGuidValidator());
            Include(new CategoryNameValidator());
        }
    }
}