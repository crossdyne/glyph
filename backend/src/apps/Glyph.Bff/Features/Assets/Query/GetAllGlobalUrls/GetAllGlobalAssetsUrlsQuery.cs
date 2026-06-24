using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Assets.Query.GetAllGlobalUrls
{
    public sealed record GetAllGlobalAssetsUrlsQuery() : IRequest<Result<List<AssetUrlResponse>>>;
}