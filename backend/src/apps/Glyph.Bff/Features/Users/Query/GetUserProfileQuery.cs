using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Authentication.Responses;

namespace Glyph.Bff.Features.Users.Query
{
    public sealed record GetUserProfileQuery() : IRequest<Result<UserProfileResponse>>;
}