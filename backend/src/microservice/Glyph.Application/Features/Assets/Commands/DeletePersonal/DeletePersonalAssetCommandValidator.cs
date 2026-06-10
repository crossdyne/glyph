using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Assets.Commands.DeletePersonal
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