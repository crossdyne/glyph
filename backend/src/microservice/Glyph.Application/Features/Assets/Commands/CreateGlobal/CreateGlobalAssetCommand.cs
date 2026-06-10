using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Application.Features.Assets.Commands.CreateGlobal
{
    public sealed record CreateGlobalAssetCommand(
        Stream FileContent,
        long SizeBytes,
        string Bucket, List<string> Folders, string FileName,
        Guid CategoryId) : IRequest<Result>;
}