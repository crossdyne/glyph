using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonalAndGlobal
{
    public sealed record GetAllPersonalAndGlobalCategoriesQuery() : IRequest<List<CategoryResponse>>;
}