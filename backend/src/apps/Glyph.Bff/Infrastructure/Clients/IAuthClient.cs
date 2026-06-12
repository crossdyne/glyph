using Crossdyne.Toolkit.Results;
using Shared.Contracts.Authentication.Requests;
using Shared.Contracts.Authentication.Responses;

namespace Glyph.Bff.Infrastructure.Clients
{
    public interface IAuthClient
    {
        Task<Result<AuthResponse>> RefreshTokens(RefreshTokensRequest request);
    }
}