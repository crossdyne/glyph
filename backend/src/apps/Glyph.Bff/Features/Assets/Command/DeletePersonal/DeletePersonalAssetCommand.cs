using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.DeletePersonal
{
    public sealed record DeletePersonalAssetCommand(string AssetId) : IRequest<Result>;
}