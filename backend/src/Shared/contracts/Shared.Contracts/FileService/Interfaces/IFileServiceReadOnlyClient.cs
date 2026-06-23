using Crossdyne.Toolkit.Results;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;

namespace Shared.Contracts.FileService.Interfaces
{
    public interface IFileServiceReadOnlyClient
    {
        Task<Result<string>> GetUrl(string bucket, string folder, string key);
        Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request);
    }
}