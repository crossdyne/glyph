using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Create
{
    public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            Include(new ProjectNameValidator());
        }
    }
}