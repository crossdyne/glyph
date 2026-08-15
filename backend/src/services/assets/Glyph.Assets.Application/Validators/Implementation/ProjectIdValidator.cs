using FluentValidation;
using Glyph.Assets.Application.Validators.Interfaces;

namespace Glyph.Assets.Application.Validators.Implementation
{
    public sealed class ProjectIdValidator : AbstractValidator<IHasProjectId>
    {
        public ProjectIdValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Идентификатор проекта не распознан.");
        }
    }
}