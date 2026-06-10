using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalCategoryCommand(Guid CategoryId) : IRequest<Result>, IHasCategoryId;
}