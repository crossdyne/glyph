using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Domain.ValueObjects.Projects;
using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Update
{
    public sealed class UpdateProjectCommandHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<UpdateProjectCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Maybe<Project> maybe = await repository.GetByAsync(x => x.Id == request.ProjectId);

                if (maybe.IsNone)
                    return Result.Failure(new Error(ErrorCode.NotFound, "Проект не найден."));

                Project project = maybe.Value;

                project.UpdateName(ProjectName.Create(request.Name));

                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Update, $"Произошла непредвиденная ошибка при обновление: {ex}"));
            }
        }
    }
}