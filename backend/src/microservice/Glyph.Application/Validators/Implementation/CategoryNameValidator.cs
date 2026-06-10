using FluentValidation;
using Glyph.Application.Validators.Interfaces;
using Glyph.Domain.ValueObjects.Categories;

namespace Glyph.Application.Validators.Implementation
{
    public sealed class CategoryNameValidator : AbstractValidator<IHasCategoryName>
    {
        public CategoryNameValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Укажите название категории")
                .Length(CategoryName.MinLength, CategoryName.MaxLength).WithMessage($"Диапазон название категории от {CategoryName.MinLength} до {CategoryName.MaxLength}");
        }
    }
}