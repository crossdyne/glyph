using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Assets.Queries.GetAllByFilter
{
    public sealed record GetAllAssetsByFilerQuery(Guid? UserId, Guid ProjectId) : IRequest<List<AssetResponse>>;
}