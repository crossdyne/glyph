using FluentValidation;
using Glyph.Assets.Application.Validators.Interfaces;
using Glyph.Assets.Domain.ValueObjects.Projects;

namespace Glyph.Assets.Application.Validators.Implementation
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