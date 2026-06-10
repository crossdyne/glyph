using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Projects.Commands.Create
{
    public sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            Include(new ProjectNameValidator());
        }
    }
}