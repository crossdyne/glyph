using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Authentication.Responses;
using Shared.Kernel.Errors;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class AuthClient(HttpClient client, IOptions<JsonSerializerOptions> jsonOptions) : IAuthClient
    {
        private readonly HttpClient _httpClient = client;
        private readonly JsonSerializerOptions _jsonOptions = jsonOptions.Value;

        public async Task<Result<AuthResponse>> RefreshTokens(Shared.Contracts.Authentication.Requests.RefreshTokensRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/refresh", request, options: _jsonOptions);
                                
                if (!response.IsSuccessStatusCode)
                {
                    var errors = await response.Content.ReadFromJsonAsync<Error[]>(_jsonOptions);
                    return Result<AuthResponse>.Failure(errors!)!;
                }

                return Result<AuthResponse?>.Success(await response.Content.ReadFromJsonAsync<AuthResponse>())!;
            }
            catch (Exception ex)
            {
                return Result<AuthResponse?>.Failure(new Error(AppErrors.Api, $"Ошибка обновление токенов: {ex.Message}"))!;
            }
        }
    }
}