using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Assets.Query.GetAllPersonalUrls
{
    public sealed record GetAllPersonalAssetsUrlsQuery() : IRequest<Result<List<AssetUrlResponse>>>;
}