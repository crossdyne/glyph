using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Categories.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalCategoryCommandValidator : AbstractValidator<UpdatePersonalCategoryCommand>
    {
        public UpdatePersonalCategoryCommandValidator()
        {
            Include(new CategoryIdValidator());
            Include(new UserIdGuidValidator());
            Include(new CategoryNameValidator());
        }
    }
}