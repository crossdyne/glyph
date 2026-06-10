using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalCategoryCommand(Guid CategoryId) : IRequest<Result>, IHasCategoryId;
}