using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Update
{
    public sealed class UpdateProjectCommandHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            Maybe<Project> maybe = await repository.GetByAsync(x => x.Id == request.ProjectId);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Проект не найден."));

            Project project = maybe.Value;

            project.UpdateName(ProjectName.Create(request.Name));

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}