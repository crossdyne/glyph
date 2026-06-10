using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Create
{
    public sealed record CreateProjectCommand(string Name) : IRequest<Result>, IHasProjectName;
}