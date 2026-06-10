using Crossdyne.Glyph.Application.Validators.Interfaces;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Validators.Implementation
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