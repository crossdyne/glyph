using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandValidator : AbstractValidator<DeleteProjectCommand>
    {
        public DeleteProjectCommandValidator()
        {
            Include(new ProjectIdValidator());
        }
    }
}