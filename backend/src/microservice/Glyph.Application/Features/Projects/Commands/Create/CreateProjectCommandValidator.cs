using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Projects.Commands.Create
{
    public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            Include(new ProjectNameValidator());
        }
    }
}