using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.UpdateGlobal
{
    public sealed record UpdateGlobalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string FileName,
        Guid AssetId) : IRequest<Result>;
}