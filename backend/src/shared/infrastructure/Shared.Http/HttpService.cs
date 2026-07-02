using System.Net.Http.Json;
using System.Text.Json;
using Shared.Http.Abstraction;

namespace Shared.Http
{
    public class HttpService<TResponse, TKey>(HttpClient http, string endpoint, JsonSerializerOptions options) : 
        BaseHttpService(http, endpoint, options),
        IHttpService<TResponse, TKey>
        where TResponse : class
    {
        public virtual async Task<TResponse?> AddAsync<TRequest>(TRequest request)
        {
            var response = await _http.PostAsJsonAsync(_endpoint, request, _jsonSerializerOptions);
            
            await HandleResponseAsync(response);

            if (response.Content.Headers.ContentLength == 0)
                return null;

             return await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions) ?? throw new JsonException("Response body is null");
        }

        public virtual async Task<TResult> AddWithResultAsync<TResult, TRequest>(TRequest request)
        {    
            var response = await _http.PostAsJsonAsync(_endpoint, request, _jsonSerializerOptions);
            
            await HandleResponseAsync(response);

            if (response.Content.Headers.ContentLength == 0)
                return default!;

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return default!;

            if (typeof(TResult) == typeof(string))
            {
                if (content.StartsWith("\"") && content.EndsWith("\""))
                {
                    using var doc = JsonDocument.Parse(content);
                    var value = doc.RootElement.GetString();
                    return (TResult)(object)value!;
                }
                else
                {
                    return (TResult)(object)content;
                }
            }

            return await response.Content.ReadFromJsonAsync<TResult>(_jsonSerializerOptions) ?? default!;
        }

        public virtual async Task CreateAsync<TRequest>(TRequest newItem) 
            => await HandleResponseAsync(await _http.PostAsJsonAsync(_endpoint, newItem, _jsonSerializerOptions));

        public virtual async Task UpdateAsync<TRequest>(TKey id, TRequest updatedItem)
            => await HandleResponseAsync(await _http.PatchAsJsonAsync( $"{_endpoint}/{id}", updatedItem, _jsonSerializerOptions));

        public virtual async Task DeleteAsync(TKey id)
            => await HandleResponseAsync(await _http.DeleteAsync($"{_endpoint}/{id}"));

        public async Task ReplaceAsync<TRequest>(TKey id, TRequest request)
            => await HandleResponseAsync(await _http.PatchAsJsonAsync($"{_endpoint}", request));

        public virtual async Task<List<TResponse>> GetAllAsync()
        {
            var response = await _http.GetAsync(_endpoint);

            await EnsureSuccessOrThrowAsync(response);

            var result = await response.Content.ReadFromJsonAsync<List<TResponse>>(_jsonSerializerOptions);
            
            return result ?? [];
        }

        public virtual async Task<TResponse?> GetByIdAsync(TKey id)
        {
            var response = await _http.GetAsync($"{_endpoint}/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            await EnsureSuccessOrThrowAsync(response);

            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonSerializerOptions);
        }
    }
}