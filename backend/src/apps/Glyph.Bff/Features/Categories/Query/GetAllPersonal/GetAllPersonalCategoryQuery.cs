using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Bff.Features.Categories.Query.GetAllPersonal
{
    public sealed record GetAllPersonalCategoryQuery(string UserId) : IRequest<List<CategoryResponse>>;
}