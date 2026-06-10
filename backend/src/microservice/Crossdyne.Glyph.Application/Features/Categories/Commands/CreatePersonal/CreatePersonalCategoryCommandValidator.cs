using Crossdyne.Glyph.Application.Validators.Implementation;
using Crossdyne.Glyph.Domain.ValueObjects.Categories;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.CreatePersonal
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