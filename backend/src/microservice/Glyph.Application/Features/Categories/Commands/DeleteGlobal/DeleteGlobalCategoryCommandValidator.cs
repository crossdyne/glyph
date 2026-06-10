using FluentValidation;
using Glyph.Application.Validators.Implementation;

namespace Glyph.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandValidator : AbstractValidator<DeleteGlobalCategoryCommand>
    {
        public DeleteGlobalCategoryCommandValidator()
        {            
            Include(new CategoryIdValidator());
        }
    }
}