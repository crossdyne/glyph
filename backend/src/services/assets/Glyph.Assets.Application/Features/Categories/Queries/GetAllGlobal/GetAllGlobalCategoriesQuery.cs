using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllGlobal
{
    public sealed record GetAllGlobalCategoriesQuery() : IRequest<List<CategoryResponse>>;
}