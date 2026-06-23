using Glyph.Assets.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllPersonalAndGlobal
{
    public sealed class GetAllPersonalAndGlobalCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllPersonalAndGlobalCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllPersonalAndGlobalCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetPersonalAndGlobal(request.UserId);
    }
}