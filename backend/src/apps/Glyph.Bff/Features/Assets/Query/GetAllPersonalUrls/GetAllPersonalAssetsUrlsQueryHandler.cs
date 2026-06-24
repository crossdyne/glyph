using Crossdyne.Toolkit.Results;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Responses;
using Shared.Contracts.FileService.Interfaces;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;
using Shared.Kernel.Errors;

namespace Glyph.Bff.Features.Assets.Query.GetAllPersonalUrls
{
    public sealed class GetAllPersonalAssetsUrlsQueryHandler(
        IFileServiceReadOnlyClient fileServiceClient, 
        IPersonalAssetClient personalAssetClient) : IRequestHandler<GetAllPersonalAssetsUrlsQuery, Result<List<AssetUrlResponse>>>
    {
        public async Task<Result<List<AssetUrlResponse>>> Handle(GetAllPersonalAssetsUrlsQuery request, CancellationToken cancellationToken)
        {
            Result<List<AssetMetadataResponse>> assetsMetadataResponse = await personalAssetClient.GetFilesMetadata();

            if (assetsMetadataResponse.IsFailure)
                return Result<List<AssetUrlResponse>>.Failure(new Error(AppErrors.Api, "Ошибка на стороне сервера"));

            List<AssetMetadataResponse> s3KeysResponse = assetsMetadataResponse.Value;

            if (s3KeysResponse == null || s3KeysResponse.Count == 0)
                return Result<List<AssetUrlResponse>>.Success([]);

            Result<BatchUrlResponse> urlsResponseResult = await fileServiceClient.GetUrls(new BatchUrlRequest([.. s3KeysResponse.Select(x => new FileRequest(x.S3Key.Bucket, x.S3Key.FolderPath, x.S3Key.Name))], null));

            if (urlsResponseResult.IsFailure)
                return Result<List<AssetUrlResponse>>.Failure(urlsResponseResult.Errors);
                
            BatchUrlResponse urlResponse = urlsResponseResult.Value;

            List<AssetUrlResponse> response = [];

            foreach (var url in urlResponse.Urls)
            {
                AssetMetadataResponse? s3Key = s3KeysResponse.FirstOrDefault(x => x.S3Key.Name == url.Key);

                if (s3Key == null)
                    continue;

                response.Add(new AssetUrlResponse(s3Key.AssetId, s3Key.AssetName, url.Url, s3Key.CategoryId, s3Key.ProjectIds, s3Key.IsPublic));
            }

            return Result<List<AssetUrlResponse>>.Success(response);
        }
    }
}