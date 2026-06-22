using System.Net.Http.Headers;
using System.Net.Http.Json;
using Crossdyne.Toolkit.Results;
using Shared.Contracts.FileService.Interfaces;
using Shared.Contracts.FileService.Requests;
using Shared.Contracts.FileService.Responses;
using Shared.Kernel.Errors;

namespace FileService.Client
{
    internal sealed class FileStorageClient(HttpClient http) : IFileServiceClient
    {
        public async Task<Result> Upload(string bucket, string folderPath, string fileName, string mimeType, Stream file)
        {
            if (file.CanSeek)
                file.Position = 0;
        
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(bucket), "bucket" },
                { new StringContent(folderPath), "folder" },
                { new StringContent(fileName), "key" },
            };

            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);

            formData.Add(streamContent, "file", fileName);

            var response = await http.PostAsync("api/files/upload", formData);

            if (!response.IsSuccessStatusCode)
                return Result.Failure(new Error(AppErrors.Http, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }

        public async Task<Result> Delete(string bucket, string folder, string key)
        {
            var response = await http.DeleteAsync($"api/files?bucket={bucket}&folder={folder}&key={key}");

            if (!response.IsSuccessStatusCode)
                return Result.Failure(new Error(AppErrors.Http, await response.Content.ReadAsStringAsync()));

            return Result.Success();
        }

        public async Task<Result<string>> GetUrl(string bucket, string folder, string key)
        {
            var response = await http.GetAsync($"api/files?bucket={bucket}&folder={folder}&key={key}");
            
            if (!response.IsSuccessStatusCode)
                return Result<string>.Failure(new Error(AppErrors.Http, await response.Content.ReadAsStringAsync()));

            return Result<string>.Success(await response.Content.ReadAsStringAsync());
        }

        public async Task<Result<BatchUrlResponse>> GetUrls(BatchUrlRequest request)
        {
            var response = await http.PostAsJsonAsync("api/files/urls", request);
            
            if (!response.IsSuccessStatusCode)
                return Result<BatchUrlResponse>.Failure(new Error(AppErrors.Http, await response.Content.ReadAsStringAsync()));

            return Result<BatchUrlResponse>.Success(await response.Content.ReadFromJsonAsync<BatchUrlResponse>());
        }
    }
}