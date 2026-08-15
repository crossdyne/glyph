using System.Text.Json;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Assets.Responses;
using Shared.Http;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class PersonalCategoriesClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : 
        HttpService<CategoryResponse, string>(http, "api/v1/personal/category", jsonOptions.Value), IPersonalCategoriesClient
    {
        public async Task<List<CategoryResponse>> GetAllPersonalAndGlobal()
        {
            var response = await _http.GetAsync($"{_endpoint}/all");

            await EnsureSuccessOrThrowAsync(response);

            var result = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>(_jsonSerializerOptions);
            
            return result ?? [];
        }
    }
}