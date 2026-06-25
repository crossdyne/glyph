using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAggregated
{
    public sealed class GetAggregatedCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAggregatedCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAggregatedCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetAggregated(request.UserId);
    }
}