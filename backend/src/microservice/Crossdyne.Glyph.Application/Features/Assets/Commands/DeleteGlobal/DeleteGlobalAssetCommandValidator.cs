using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalAssetCommandValidator : AbstractValidator<DeleteGlobalAssetCommand>
    {
        public DeleteGlobalAssetCommandValidator()
        {
            Include(new AssetIdValidator());
        }
    }
}