using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Delete
{
    public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest<Result>, IHasProjectId;
}