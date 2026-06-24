using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllGlobal
{
    public sealed record GetAllGlobalCategoriesQuery() : IRequest<List<CategoryResponse>>;
}