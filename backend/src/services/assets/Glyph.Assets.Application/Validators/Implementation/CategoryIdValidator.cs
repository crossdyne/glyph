using FluentValidation;
using Glyph.Assets.Application.Validators.Interfaces;

namespace Glyph.Assets.Application.Validators.Implementation
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