using Crossdyne.Toolkit.Results;
using Shared.Contracts.Authentication.Requests;
using Shared.Contracts.Authentication.Responses;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IAuthClient
    {
        Task<Result<AuthResponse>> RefreshTokens(RefreshTokensRequest request);
    }
}