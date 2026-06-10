using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.UpdatePersonal
{
    public sealed record UpdatePersonalCategoryCommand(Guid CategoryId, Guid UserId, string Name) : IRequest<Result>, IHasCategoryId, IHasUserIdGuid, IHasCategoryName;
}