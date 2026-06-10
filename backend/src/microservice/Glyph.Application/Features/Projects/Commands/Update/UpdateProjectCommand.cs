using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Projects.Commands.Update
{
    public sealed record UpdateProjectCommand(Guid ProjectId, string Name) : IRequest<Result>, IHasProjectId, IHasProjectName;
}