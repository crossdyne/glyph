using Crossdyne.Glyph.Application.Validators.Implementation;
using FluentValidation;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed class DeleteGlobalCategoryCommandValidator : AbstractValidator<DeleteGlobalCategoryCommand>
    {
        public DeleteGlobalCategoryCommandValidator()
        {            
            Include(new CategoryIdValidator());
        }
    }
}