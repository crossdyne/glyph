using System.Text.Json;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Assets.Responses;
using Shared.Http;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class GlobalCategoriesClient(HttpClient http, IOptions<JsonSerializerOptions> options) 
    : HttpService<CategoryResponse, string>(http, "api/v1/global/category", options.Value), IGlobalCategoriesClient
    {
        
    }
}