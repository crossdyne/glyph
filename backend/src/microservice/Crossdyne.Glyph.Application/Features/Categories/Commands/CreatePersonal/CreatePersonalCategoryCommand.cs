using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.CreatePersonal
{
    public sealed record CreatePersonalCategoryCommand(Guid UserId, string Name) : IRequest<Result>, IHasUserIdGuid, IHasCategoryName;
}