using Crossdyne.Toolkit.Results;
using Shared.Contracts.Assets.Responses;
using Shared.Http.Abstraction;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IGlobalAssetClient : IGetAllHttp<AssetResponse>, IDeleteHttp<string>
    {
        Task<Result<string>> Create(
            string bucket, 
            IReadOnlyCollection<string> folders,
            string fileName,
            string categoryId,
            string projectIdsJson,
            Stream file,
            string Name);
        Task<Result> UpdateAsync(string assetId, string assetName, Stream? file, string? fileName, string categoryId);

        Task<Result<List<AssetMetadataResponse>>> GetFilesMetadata();
    }
}