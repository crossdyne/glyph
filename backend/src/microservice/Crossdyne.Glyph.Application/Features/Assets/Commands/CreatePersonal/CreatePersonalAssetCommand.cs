using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Assets.Commands.CreatePersonal
{
    public sealed record CreatePersonalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string Bucket, List<string> Folders, string FileName,
        Guid CategoryId,
        IReadOnlyCollection<string> ProjectIds,
        Guid UserId
    ) : IRequest<Result>;
}