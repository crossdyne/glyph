using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Responses;
using Shared.Http;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class PersonalAssetClient(HttpClient http, IOptions<JsonSerializerOptions> options) : 
        HttpService<AssetResponse, string>(http, "api/v1/personal/asset", options.Value), IPersonalAssetClient
    {
        public async Task<Result<string>> Create(
            string bucket, 
            IReadOnlyCollection<string> folders,
            string fileName,
            string categoryId,
            string projectIdsJson,
            Stream file,
            string assetName)
        {
            using var content = new MultipartFormDataContent();

            string foldersJson = JsonSerializer.Serialize(folders); 

            content.Add(new StringContent(foldersJson), "FoldersJson");
            content.Add(new StringContent(bucket), "Bucket");
            content.Add(new StringContent(fileName), "FileName");
            content.Add(new StringContent(categoryId.ToString()), "CategoryId");
            content.Add(new StringContent(projectIdsJson), "ProjectIdsJson");
            content.Add(new StringContent(assetName), "AssetName");

            if (file.CanSeek)
                file.Position = 0;

            content.Add(new StreamContent(file), "File", fileName);

            var response = await _http.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
                return Result<string>.Failure(new Error(ErrorCode.Server, $"HTTP {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}"));

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<Result> UpdateAsync(string assetId, string assetName, Stream file, string fileName, string categoryId)
        {
            using var content = new MultipartFormDataContent();

            content.Add(new StringContent(assetId), "AssetId");
            content.Add(new StringContent(assetName), "AssetName");
            content.Add(new StringContent(categoryId), "CategoryId");

            if (file.CanSeek)
                file.Position = 0;

            content.Add(new StreamContent(file), "File", fileName);

            var response = await _http.PutAsync(_endpoint, content);
            
            if (!response.IsSuccessStatusCode)
            {
                var error = $"HTTP {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}";
                Console.WriteLine(error);
                return Result<string>.Failure(new Error(ErrorCode.Server, error));
            }
                

            return Result.Success();
        }

        public async Task<Result<List<AssetMetadataResponse>>> GetFilesMetadata()
        {
            var response = await _http.GetAsync($"{_endpoint}/metadata/many");

            if (!response.IsSuccessStatusCode)
                return Result<List<AssetMetadataResponse>>.Failure(new Error(ErrorCode.Server, $"HTTP {(int)response.StatusCode}: {await response.Content.ReadAsStringAsync()}"));

            return Result<List<AssetMetadataResponse>>.Success(await response.Content.ReadFromJsonAsync<List<AssetMetadataResponse>>(_jsonSerializerOptions));
        }
    }
}