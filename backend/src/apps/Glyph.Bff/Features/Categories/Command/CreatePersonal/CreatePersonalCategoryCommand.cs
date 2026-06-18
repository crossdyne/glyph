using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.CreatePersonal
{
    public sealed record CreatePersonalCategoryCommand(string Name) : IRequest<Result<string>>;
}