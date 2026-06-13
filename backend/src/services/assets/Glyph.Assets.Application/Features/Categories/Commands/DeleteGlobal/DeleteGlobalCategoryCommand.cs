using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.DeleteGlobal
{
    public sealed record DeleteGlobalCategoryCommand(Guid CategoryId) : IRequest<Result>, IHasCategoryId;
}