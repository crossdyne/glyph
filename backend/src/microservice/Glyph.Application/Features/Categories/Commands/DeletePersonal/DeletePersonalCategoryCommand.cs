using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.DeletePersonal
{
    public sealed record DeletePersonalCategoryCommand(Guid CategoryId, Guid UserId) : IRequest<Result>, IHasCategoryId, IHasUserIdGuid;
}