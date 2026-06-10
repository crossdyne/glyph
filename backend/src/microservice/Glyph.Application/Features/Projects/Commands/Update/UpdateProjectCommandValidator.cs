using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Projects.Commands.Update
{
    public sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {
        public UpdateProjectCommandValidator()
        {
            Include(new ProjectIdValidator());
            Include(new ProjectNameValidator());
        }
    }
}