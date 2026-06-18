using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonal
{
    public sealed class GetAllPersonalCategoryQueryHandler(IPersonalCategoriesClient client) : IRequestHandler<GetAllPersonalCategoryQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllPersonalCategoryQuery request, CancellationToken cancellationToken)
            => await client.GetAllAsync();
    }
}