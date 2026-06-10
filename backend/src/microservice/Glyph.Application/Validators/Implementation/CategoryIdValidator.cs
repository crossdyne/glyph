using FluentValidation;
using Glyph.Application.Validators.Interfaces;

namespace Glyph.Application.Validators.Implementation
{
    public sealed class CategoryIdValidator : AbstractValidator<IHasCategoryId>
    {
        public CategoryIdValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Не удалось определить идентификатор категории");
        }
    }
}