using Crossdyne.Toolkit.Results;
using MediatR;

namespace Glyph.Bff.Features.Categories.Command.DeletePersonal
{
    public sealed record DeletePersonalCategoryCommand(string CategoryId) : IRequest<Result>;
}