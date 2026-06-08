using Crossdyne.Glyph.Application.Validators.Interfaces;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Validators.Implementation
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