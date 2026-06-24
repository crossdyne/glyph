using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Assets.Application.Features.Categories.Queries.GetAllPersonal
{
    public sealed record GetAllPersonalCategoriesQuery(Guid UserId) : IRequest<Result<List<CategoryResponse>>>;
}