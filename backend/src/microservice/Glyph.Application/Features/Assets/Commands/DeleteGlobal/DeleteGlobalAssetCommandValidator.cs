using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Assets.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalAssetCommandValidator : AbstractValidator<DeleteGlobalAssetCommand>
    {
        public DeleteGlobalAssetCommandValidator()
        {
            Include(new AssetIdValidator());
        }
    }
}