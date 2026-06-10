using FluentValidation;
using Glyph.Application.Validators.Interfaces;

namespace Glyph.Application.Validators.Implementation
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