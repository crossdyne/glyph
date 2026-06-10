using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Categories.Commands.CreateGlobal
{
    public sealed record CreateGlobalCategoryCommand(string Name) : IRequest<Result>, IHasCategoryName;
}