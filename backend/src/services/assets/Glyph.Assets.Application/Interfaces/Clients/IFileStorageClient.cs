using Crossdyne.Toolkit.Results;
using Glyph.Assets.Domain.ValueObjects.Assets;

namespace Glyph.Assets.Application.Interfaces.Clients
{
    public interface IFileStorageClient
    {
        Task<Result> Upload(S3Key s3Key, MimeType mimeType, Stream file);
        Task<Result> Delete(string bucket, string folder, string key);
        Task<Result<string>> GetUrl(string bucket, string folder, string key);
    }
}