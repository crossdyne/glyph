using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.UpdatePersonal
{
    public sealed record UpdatePersonalCategoryCommand(string CategoryId, string Name) : IRequest<Result>;
}