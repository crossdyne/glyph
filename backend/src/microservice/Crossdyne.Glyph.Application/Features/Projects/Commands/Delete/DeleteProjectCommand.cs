using Crossdyne.Glyph.Application.Validators.Interfaces;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Delete
{
    public sealed record DeleteProjectCommand(Guid ProjectId) : IRequest<Result>, IHasProjectId;
}