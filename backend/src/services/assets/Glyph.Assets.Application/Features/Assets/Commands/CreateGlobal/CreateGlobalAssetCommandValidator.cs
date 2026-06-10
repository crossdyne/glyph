using FluentValidation;

namespace Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal
{
    public sealed class CreateGlobalAssetCommandValidator : AbstractValidator<CreateGlobalAssetCommand>
    {
        public CreateGlobalAssetCommandValidator()
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