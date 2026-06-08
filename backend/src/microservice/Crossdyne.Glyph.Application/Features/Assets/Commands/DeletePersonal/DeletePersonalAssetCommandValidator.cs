using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.DeletePersonal
{
    public sealed class DeletePersonalAssetCommandValidator : AbstractValidator<DeletePersonalAssetCommand>
    {
        public DeletePersonalAssetCommandValidator()
        {
            Include(new UserIdGuidValidator());
            Include(new AssetIdValidator());
        }
    }
}