using System.Text.Json;
using Crossdyne.Toolkit.Results;
using Glyph.Bff.Interfaces.Clients;
using Microsoft.Extensions.Options;
using Shared.Contracts.Authentication.Responses;
using Shared.Kernel.Errors;

namespace Glyph.Bff.Infrastructure.Clients
{
    public sealed class UserManagementClient(HttpClient client, IOptions<JsonSerializerOptions> jsonOptions) : IUserManagementClient
    {
        public async Task<Result<UserProfileResponse>> Me()
        {
            try
            {
                var response = await client.GetAsync($"api/v1/users/me");

                if (!response.IsSuccessStatusCode)
                {
                    var errors = await response.Content.ReadFromJsonAsync<Error[]>(jsonOptions.Value);
                    return Result<UserProfileResponse>.Failure(errors!)!;
                }

                return Result<UserProfileResponse>.Success(await response.Content.ReadFromJsonAsync<UserProfileResponse>())!;
            }
            catch (Exception ex)
            {
                return Result<UserProfileResponse>.Failure(new Error(AppErrors.Api, $"Ошибка обновление токенов: {ex.Message}"))!;
            }
        }
    }
}