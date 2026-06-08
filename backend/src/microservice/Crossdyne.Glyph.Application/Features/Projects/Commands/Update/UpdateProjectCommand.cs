using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Update
{
    public sealed record UpdateProjectCommand(Guid ProjectId, string Name) : IRequest<Result>, IHasProjectId, IHasProjectName;
}