using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Assets.Application.Features.Assets.Commands.CreateGlobal
{
    public sealed record CreateGlobalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string Bucket, List<string> Folders, string FileName,
        Guid CategoryId,
        IReadOnlyCollection<string> ProjectIds,
        string AssetName) : IRequest<Result<string>>;
}