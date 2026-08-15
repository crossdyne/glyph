using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Assets.Commands.DeletePersonal
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