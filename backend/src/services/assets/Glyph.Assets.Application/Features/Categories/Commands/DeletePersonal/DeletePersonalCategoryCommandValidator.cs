using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.DeletePersonal
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