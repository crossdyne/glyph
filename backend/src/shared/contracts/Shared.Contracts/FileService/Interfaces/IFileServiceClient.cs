using Crossdyne.Toolkit.Results;

namespace Shared.Contracts.FileService.Interfaces
{
    public interface IFileServiceClient : IFileServiceReadOnlyClient
    {
        Task<Result> Upload(string bucket, string folderPath, string fileName, string mimeType, Stream file);
        Task<Result> Delete(string bucket, string folder, string key);
    }
}