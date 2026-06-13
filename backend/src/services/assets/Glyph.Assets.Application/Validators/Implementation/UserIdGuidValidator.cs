using FluentValidation;
using Glyph.Assets.Application.Validators.Interfaces;

namespace Glyph.Assets.Application.Validators.Implementation
{
    public sealed class UserIdGuidValidator : AbstractValidator<IHasUserIdGuid>
    {
        public UserIdGuidValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Идентификатор пользователя не распознан.");
        }
    }
}