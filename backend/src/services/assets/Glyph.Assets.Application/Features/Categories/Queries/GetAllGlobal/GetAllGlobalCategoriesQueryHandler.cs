using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllGlobal
{
    public sealed class GetAllGlobalCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllGlobalCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllGlobalCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetAllGlobalAsync();
    }
}