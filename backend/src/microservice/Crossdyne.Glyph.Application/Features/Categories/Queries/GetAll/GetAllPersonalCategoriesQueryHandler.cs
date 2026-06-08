using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Categories.Queries.GetAll
{
    public sealed class GetAllPersonalCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllPersonalCategoriesQuery, Result<List<CategoryResponse>>>
    {
        public async Task<Result<List<CategoryResponse>>> Handle(GetAllPersonalCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetAllAsync(request.UserId);
    }
}