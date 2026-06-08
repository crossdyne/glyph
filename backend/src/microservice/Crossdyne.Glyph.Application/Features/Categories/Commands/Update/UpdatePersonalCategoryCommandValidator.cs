using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.Update
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