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
            Stream file)
        {
            using var content = new MultipartFormDataContent();

            string foldersJson = JsonSerializer.Serialize(folders); 

            content.Add(new StringContent(foldersJson), "FoldersJson");
            content.Add(new StringContent(bucket), "Bucket");
            content.Add(new StringContent(fileName), "FileName");
            content.Add(new StringContent(categoryId.ToString()), "CategoryId");
            content.Add(new StringContent(projectIdsJson), "ProjectIdsJson");

            if (file.CanSeek)
                file.Position = 0;

            var fileContent = new StreamContent(file);
            content.Add(fileContent, "File", fileName);

            var response = await _http.PostAsync(_endpoint, content);

            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[BFF] Ошибка {response.StatusCode}: {responseBody}");
                return Result<string>.Failure(new Error(ErrorCode.Server, $"HTTP {(int)response.StatusCode}: {responseBody}"));
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}