using Crossdyne.Toolkit.Results;
using Glyph.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Application.Features.Categories.Queries.GetAll
{
    public sealed class GetAllPersonalCategoriesQueryHandler(ICategoryRepository repository) : IRequestHandler<GetAllPersonalCategoriesQuery, Result<List<CategoryResponse>>>
    {
        public async Task<Result<List<CategoryResponse>>> Handle(GetAllPersonalCategoriesQuery request, CancellationToken cancellationToken)
            => await repository.GetAllAsync(request.UserId);
    }
}