using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllGlobal
{
    public sealed class GetAllGlobalCategoriesQueryHandler(IGlobalCategoriesClient client) : IRequestHandler<GetAllGlobalCategoriesQuery, List<CategoryResponse>>
    {
        public async Task<List<CategoryResponse>> Handle(GetAllGlobalCategoriesQuery request, CancellationToken cancellationToken)
            => await client.GetAllAsync();
    }
}