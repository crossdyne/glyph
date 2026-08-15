using FluentValidation;
using Glyph.Assets.Application.Validators.Interfaces;

namespace Glyph.Assets.Application.Validators.Implementation
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