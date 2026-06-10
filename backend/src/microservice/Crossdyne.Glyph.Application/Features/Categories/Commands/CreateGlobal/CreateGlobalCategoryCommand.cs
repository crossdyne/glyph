using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Categories.Commands.CreateGlobal
{
    public sealed record CreateGlobalCategoryCommand(string Name) : IRequest<Result>, IHasCategoryName;
}