using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Categories.Commands.CreatePersonal
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