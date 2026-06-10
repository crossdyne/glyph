using FluentValidation;

namespace Glyph.Assets.Application.Features.Assets.Commands.UpdatePersonal
{
    public sealed class UpdatePersonalAssetCommandValidator : AbstractValidator<UpdatePersonalAssetCommand>
    {
        public UpdatePersonalAssetCommandValidator()
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("Название файла обязательно.");
        }
    }
}