using Crossdyne.Glyph.Application.Validators.Interfaces;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Validators.Implementation
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