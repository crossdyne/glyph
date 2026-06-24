using Crossdyne.Toolkit.Results;
using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Authentication.Responses;

namespace Glyph.Bff.Features.Users.Query
{
    public sealed class GetUserProfileQueryHandler(IUserManagementClient client) : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponse>>
    {
        public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
            => await client.Me();
    }
}