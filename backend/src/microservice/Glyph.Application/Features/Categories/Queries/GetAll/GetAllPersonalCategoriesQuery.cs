using Crossdyne.Toolkit.Results;
using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Application.Features.Categories.Queries.GetAll
{
    public sealed record GetAllPersonalCategoriesQuery(Guid UserId) : IRequest<Result<List<CategoryResponse>>>;
}