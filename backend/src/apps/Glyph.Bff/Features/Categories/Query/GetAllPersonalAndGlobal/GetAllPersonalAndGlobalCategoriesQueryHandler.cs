using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonalAndGlobal
{
    public sealed class GetAllPersonalAndGlobalCategoriesQueryHandler(IPersonalCategoriesClient client) : IRequestHandler<GetAllPersonalAndGlobalCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllPersonalAndGlobalCategoriesQuery request, CancellationToken cancellationToken)
            => await client.GetAllPersonalAndGlobal();
    }
}