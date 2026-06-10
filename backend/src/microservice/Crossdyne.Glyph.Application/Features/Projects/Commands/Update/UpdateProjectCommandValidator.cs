using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Update
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