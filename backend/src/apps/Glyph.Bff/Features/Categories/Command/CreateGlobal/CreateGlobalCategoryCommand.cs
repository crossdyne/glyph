using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.CreateGlobal
{
    public sealed record CreateGlobalCategoryCommand(string Name) : IRequest<Result<string>>;
}