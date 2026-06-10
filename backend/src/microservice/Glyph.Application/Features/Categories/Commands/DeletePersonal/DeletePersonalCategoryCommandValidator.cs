using FluentValidation;
using Glyph.Application.Validators.Implementation;
namespace Glyph.Application.Features.Categories.Commands.DeletePersonal
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