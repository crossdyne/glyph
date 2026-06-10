using System.Net.Http.Headers;
using Crossdyne.Toolkit.Results;
using Glyph.Application.Errors;
using Glyph.Application.Interfaces.Clients;
using Glyph.Domain.ValueObjects.Assets;

namespace Glyph.Infrastructure.Clients
{
    internal sealed class FileStorageClient(HttpClient http) : IFileStorageClient
    {
        public async Task<Result> Upload(S3Key s3key, MimeType mimeType, Stream file)
        {
            if (file.CanSeek)
                file.Position = 0;
        
            using var formData = new MultipartFormDataContent
            {
                { new StringContent(s3key.Bucket), "bucket" },
                { new StringContent(s3key.FolderPath), "folder" },
                { new StringContent(s3key.FileName), "key" },
            };

            var streamContent = new StreamContent(file);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType.Value);

            formData.Add(streamContent, "file", s3key.FileName);

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
    }
}