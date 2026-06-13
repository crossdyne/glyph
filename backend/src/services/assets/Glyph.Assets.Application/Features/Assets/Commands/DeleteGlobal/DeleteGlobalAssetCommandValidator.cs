using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalAssetCommandValidator : AbstractValidator<DeleteGlobalAssetCommand>
    {
        public DeleteGlobalAssetCommandValidator()
        {
            Include(new AssetIdValidator());
        }
    }
}