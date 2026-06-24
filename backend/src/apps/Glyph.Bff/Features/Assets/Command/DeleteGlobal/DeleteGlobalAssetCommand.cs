using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.DeleteGlobal
{
    public sealed record DeleteGlobalAssetCommand(string AssetId) : IRequest<Result>;
}