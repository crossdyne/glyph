using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.UpdateGlobal
{
    public sealed record UpdateGlobalCategoryCommand(Guid CategoryId, string Name) : IRequest<Result>, IHasCategoryId, IHasCategoryName;
}