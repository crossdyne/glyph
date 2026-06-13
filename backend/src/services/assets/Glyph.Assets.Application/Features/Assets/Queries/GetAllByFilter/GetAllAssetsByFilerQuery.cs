using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Features.Assets.Queries.GetAllByFilter
{
    public sealed record GetAllAssetsByFilerQuery(Guid? UserId, Guid ProjectId) : IRequest<List<AssetResponse>>;
}