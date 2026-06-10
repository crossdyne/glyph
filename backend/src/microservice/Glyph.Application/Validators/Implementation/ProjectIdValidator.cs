using FluentValidation;
using Glyph.Application.Validators.Interfaces;

namespace Glyph.Application.Validators.Implementation
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