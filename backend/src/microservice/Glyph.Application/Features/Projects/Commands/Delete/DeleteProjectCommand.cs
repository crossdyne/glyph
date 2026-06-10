using Crossdyne.Toolkit.Results;
using Glyph.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Application.Features.Projects.Commands.Delete
{
    public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest<Result>, IHasProjectId;
}