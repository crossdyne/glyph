using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.CreateGlobal
{
    public sealed record CreateGlobalAssetCommand(Stream File, string FileName, string CategoryId, string ProjectIdsJson, string AssetName) : IRequest<Result<string>>;
}