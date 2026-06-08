using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.DeletePersonal
{
    public sealed record DeletePersonalCategoryCommand(Guid CategoryId, Guid UserId) : IRequest<Result>, IHasCategoryId, IHasUserIdGuid;
}