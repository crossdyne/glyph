using Crossdyne.Toolkit.Results;
using Shared.Contracts.Responses;
using Shared.Http.Abstraction;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IPersonalAssetClient : IGetAllHttp<AssetResponse>, IDeleteHttp<string>
    {
        Task<Result<string>> Create(
            string bucket, 
            IReadOnlyCollection<string> folders,
            string fileName,
            string categoryId,
            string projectIdsJson,
            Stream file);
            
        Task<Result<List<AssetMetadataResponse>>> GetFilesMetadata();
    }
}