using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Update
{
    public sealed record UpdateProjectCommand(Guid ProjectId, string Name) : IRequest<Result>, IHasProjectId, IHasProjectName;
}