using System.Text.Json;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Responses;
using Shared.Http;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class PersonalCategoriesClient(HttpClient http, IOptions<JsonSerializerOptions> jsonOptions) : 
        HttpService<CategoryResponse, string>(http, "api/v1/personal/category", jsonOptions.Value), IPersonalCategoriesClient
    {
    }
}