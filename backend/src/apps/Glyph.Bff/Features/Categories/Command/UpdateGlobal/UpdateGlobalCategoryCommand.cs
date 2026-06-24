using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.UpdateGlobal
{
    public sealed record UpdateGlobalCategoryCommand(string AssetId, string Name) : IRequest<Result>;
}