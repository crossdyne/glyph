using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.UpdatePersonal
{
    public sealed record UpdatePersonalAssetCommand(string AssetId, string AssetName, Stream File, string FileName) : IRequest<Result>;
}