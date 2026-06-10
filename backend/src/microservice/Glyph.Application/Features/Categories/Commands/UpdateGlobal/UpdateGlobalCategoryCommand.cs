using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.UpdateGlobal
{
    public sealed record UpdateGlobalCategoryCommand(Guid CategoryId, string Name) : IRequest<Result>, IHasCategoryId, IHasCategoryName;
}