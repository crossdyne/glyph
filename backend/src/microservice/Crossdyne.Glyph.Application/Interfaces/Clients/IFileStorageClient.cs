using Crossdyne.Glyph.Domain.ValueObjects.Assets;
using Crossdyne.Toolkit.Results;

namespace Crossdyne.Glyph.Application.Interfaces.Clients
{
    public interface IFileStorageClient
    {
        Task<Result> Upload(S3Key s3Key, MimeType mimeType, Stream file);
        Task<Result> Delete(string bucket, string folder, string key);
        Task<Result<string>> GetUrl(string bucket, string folder, string key);
    }
}