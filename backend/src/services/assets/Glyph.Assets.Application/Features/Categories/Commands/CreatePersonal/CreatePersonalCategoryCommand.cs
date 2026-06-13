using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreatePersonal
{
    public sealed record CreatePersonalCategoryCommand(Guid UserId, string Name) : IRequest<Result>, IHasUserIdGuid, IHasCategoryName;
}