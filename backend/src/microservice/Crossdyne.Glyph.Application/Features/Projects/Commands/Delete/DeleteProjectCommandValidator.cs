using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            Include(new ProjectIdValidator());
        }
    }
}