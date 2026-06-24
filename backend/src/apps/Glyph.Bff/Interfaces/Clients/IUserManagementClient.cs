using Crossdyne.Toolkit.Results;
using Shared.Contracts.Authentication.Responses;

namespace Glyph.Bff.Interfaces.Clients
{
    public interface IUserManagementClient
    {
        Task<Result<UserProfileResponse>> Me();
    }
}