using System.Text.Json;

namespace Shared.Http
{
    public abstract class BaseHttpService(HttpClient http, string endpoint, JsonSerializerOptions options)
    {
        protected readonly HttpClient _http = http;
        protected readonly string _endpoint = endpoint;
        protected readonly JsonSerializerOptions _jsonSerializerOptions = options;
        
        protected async Task HandleResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) 
                return;

            var errorBody = await response.Content.ReadAsStringAsync();
        
            throw new HttpOperationException(response.StatusCode, errorBody);
        }

        protected async Task EnsureSuccessOrThrowAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var errorBody = await response.Content.ReadAsStringAsync();

            throw new HttpOperationException(response.StatusCode, errorBody);
        }
    }
}