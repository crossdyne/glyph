using Crossdyne.Glyph.Domain.ValueObjects.Assets;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.CreatePersonal
{
    public class CreatePersonalAssetCommandValidator : AbstractValidator<CreatePersonalAssetCommand>
    {
        public CreatePersonalAssetCommandValidator()
        {
            RuleFor(x => x.Bucket)
                .NotEmpty().WithMessage("Бакет обязателен.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("Название файла обязательно.");

            RuleFor(x => x.CategoryId)
                .NotEqual(Guid.Empty).WithMessage("Категория обязательна.");
        }
    }
}