using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.UpdateGlobal
{
    public sealed record UpdateGlobalCategoryCommand(Guid CategoryId, string Name) : IRequest<Result>, IHasCategoryId, IHasCategoryName;
}