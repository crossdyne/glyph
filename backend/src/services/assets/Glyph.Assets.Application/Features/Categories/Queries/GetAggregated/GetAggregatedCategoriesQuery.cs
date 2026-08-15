using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAggregated
{
    public sealed record GetAggregatedCategoriesQuery(Guid UserId) : IRequest<List<CategoryResponse>>;
}