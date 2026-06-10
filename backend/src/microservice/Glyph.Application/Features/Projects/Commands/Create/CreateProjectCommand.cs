using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Projects.Commands.Create
{
    public sealed record CreateProjectCommand(string Name) : IRequest<Result>, IHasProjectName;
}