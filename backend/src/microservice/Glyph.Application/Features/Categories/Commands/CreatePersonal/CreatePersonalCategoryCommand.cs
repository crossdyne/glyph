using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.CreatePersonal
{
    public sealed record CreatePersonalCategoryCommand(Guid UserId, string Name) : IRequest<Result>, IHasUserIdGuid, IHasCategoryName;
}