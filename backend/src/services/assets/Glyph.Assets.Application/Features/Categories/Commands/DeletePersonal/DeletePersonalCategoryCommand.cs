using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.DeletePersonal
{
    public sealed record DeletePersonalCategoryCommand(Guid CategoryId, Guid UserId) : IRequest<Result>, IHasCategoryId, IHasUserIdGuid;
}