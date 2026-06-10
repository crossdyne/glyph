using FluentValidation;
using Glyph.Application.Validators.Interfaces;

namespace Glyph.Application.Validators.Implementation
{
    public sealed class AssetIdValidator : AbstractValidator<IHasAssetId>
    {
        public AssetIdValidator()
        {
            RuleFor(x => x.AssetId)
                .NotEmpty().WithMessage("Идентификатор ассета не был определен.");
        }
    }
}