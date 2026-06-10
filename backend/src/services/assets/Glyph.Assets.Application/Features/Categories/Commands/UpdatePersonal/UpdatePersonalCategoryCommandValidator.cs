using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.UpdatePersonal
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