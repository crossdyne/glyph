using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Categories.Queries.GetAllGlobal
{
    public sealed record GetAllGlobalCategoriesQuery() : IRequest<List<CategoryResponse>>;
}