using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Assets.Command.CreatePersonal
{
    public sealed record CreatePersonalAssetCommand(Stream File, string FileName, string CategoryId, string ProjectIdsJson, string AssetName) : IRequest<Result<string>>;
}