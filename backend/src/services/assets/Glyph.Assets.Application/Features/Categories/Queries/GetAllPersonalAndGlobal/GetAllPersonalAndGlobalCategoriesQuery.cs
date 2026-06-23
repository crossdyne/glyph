using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllPersonalAndGlobal
{
    public sealed record GetAllPersonalAndGlobalCategoriesQuery(Guid UserId) : IRequest<List<CategoryResponse>>;
}