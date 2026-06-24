using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.DeleteGlobal
{
    public sealed record DeleteGlobalCategoryCommand(string AssetId) : IRequest<Result>;
}