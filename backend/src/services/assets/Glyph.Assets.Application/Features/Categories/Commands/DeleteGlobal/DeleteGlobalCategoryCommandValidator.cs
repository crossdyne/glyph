using FluentValidation;
using Glyph.Assets.Application.Validators.Implementation;

namespace Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandValidator : AbstractValidator<DeleteGlobalCategoryCommand>
    {
        public DeleteGlobalCategoryCommandValidator()
        {            
            Include(new CategoryIdValidator());
        }
    }
}