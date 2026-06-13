using FluentValidation;

namespace Glyph.Assets.Application.Features.Assets.Commands.UpdateGlobal
{
    public sealed class UpdateGlobalAssetCommandValidator : AbstractValidator<UpdateGlobalAssetCommand>
    {
        public UpdateGlobalAssetCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("Название файла обязательно.");
        }
    }
}