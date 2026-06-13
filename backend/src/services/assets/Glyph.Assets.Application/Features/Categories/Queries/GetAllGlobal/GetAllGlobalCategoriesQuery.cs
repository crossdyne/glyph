using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllGlobal
{
    public sealed record GetAllGlobalCategoriesQuery() : IRequest<List<CategoryResponse>>;
}