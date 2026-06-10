using Crossdyne.Glyph.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Categories.Queries.GetAllGlobal
{
    public sealed class GetAllGlobalCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllGlobalCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllGlobalCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetAllGlobalAsync();
    }
}