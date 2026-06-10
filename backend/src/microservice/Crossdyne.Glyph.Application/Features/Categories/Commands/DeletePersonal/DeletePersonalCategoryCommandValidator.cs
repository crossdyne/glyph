using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.DeletePersonal
{
    public sealed class DeletePersonalCategoryCommandValidator : AbstractValidator<DeletePersonalCategoryCommand>
    {
        public DeletePersonalCategoryCommandValidator()
        {
            Include(new UserIdGuidValidator());
            Include(new CategoryIdValidator());
        }
    }
}