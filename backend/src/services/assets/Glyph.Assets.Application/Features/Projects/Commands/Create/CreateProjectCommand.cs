using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Validators.Interfaces;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Create
{
    public sealed record CreateProjectCommand(string Name) : IRequest<Result>, IHasProjectName;
}