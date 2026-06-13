using System.Text.Json;
using Microsoft.Extensions.Options;
using Shared.Contracts.Responses;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class PersonalCategoriesClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : IPersonalCategoriesClient
    {
        private readonly JsonSerializerOptions _jsonOptions = jsonOptions.Value;

        public async Task<List<CategoryResponse>> GetAll(string userId)
        {
            var response = await http.GetAsync($"api/v1/personal/category/{userId}");

            response.EnsureSuccessStatusCode();

            var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponse>>(_jsonOptions);

            return categories ?? new List<CategoryResponse>();
        }
    }
}