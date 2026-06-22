using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.UpdatePersonal
{
    public sealed record UpdatePersonalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string FileName,
        string AssetName,
        Guid AssetId,
        Guid UserId) : IRequest<Result>;
}