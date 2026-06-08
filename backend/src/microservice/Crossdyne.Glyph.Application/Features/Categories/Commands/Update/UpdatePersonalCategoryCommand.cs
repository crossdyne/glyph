using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.Update
{
    public sealed record UpdatePersonalCategoryCommand(Guid CategoryId, Guid UserId, string Name) : IRequest<Result>, IHasCategoryId, IHasUserIdGuid, IHasCategoryName;
}