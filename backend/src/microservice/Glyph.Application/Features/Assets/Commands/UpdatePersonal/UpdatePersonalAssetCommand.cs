using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.UpdatePersonal
{
    public sealed record UpdatePersonalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string FileName,
        Guid AssetId,
        Guid UserId) : IRequest<Result>;
}