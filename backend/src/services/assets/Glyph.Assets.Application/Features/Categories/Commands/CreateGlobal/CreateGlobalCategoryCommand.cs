using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Categories.Commands.CreateGlobal
{
    public sealed record CreateGlobalCategoryCommand(string Name) : IRequest<Result<string>>, IHasCategoryName;
}