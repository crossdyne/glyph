using FluentValidation;
using Glyph.Application.Validators.Interfaces;
using Glyph.Domain.ValueObjects.Projects;

namespace Glyph.Application.Validators.Implementation
{
    public sealed class ProjectNameValidator : AbstractValidator<IHasProjectName>
    {
        public ProjectNameValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Укажите название проекта.")
                .Length(ProjectName.MinLength, ProjectName.MaxLength).WithMessage($"Длинна названия должна составлять от {ProjectName.MinLength} до {ProjectName.MaxLength}");            
        }
    }
}