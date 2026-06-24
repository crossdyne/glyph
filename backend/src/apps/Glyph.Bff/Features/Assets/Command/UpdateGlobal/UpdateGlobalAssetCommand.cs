using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.UpdateGlobal
{
    public sealed record UpdateGlobalAssetCommand(string AssetId, string AssetName, Stream File, string FileName, string CategoryId) : IRequest<Result>;
}