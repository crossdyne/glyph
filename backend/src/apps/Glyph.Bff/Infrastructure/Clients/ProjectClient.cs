using System.Text.Json;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Responses;
using Shared.Http;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class ProjectClient(HttpClient client, IOptions<JsonSerializerOptions> options) :
     HttpService<ProjectResponse, string>(client, "api/v1/project", options.Value), IProjectClient
    {
        
    }
}