using Crossdyne.Toolkit.Primitives;
using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Delete
{
    public sealed class DeleteProjectCommandHandler(
        IProjectRepository projectRepository, 
        IAssetRepository assetRepository,
        IUnitOfWork unitOfWork) : IRequestHandler<DeleteProjectCommand, Result>
    {
        public async Task<Result> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            Maybe<Project> maybe = await projectRepository.GetByAsync(x => x.Id == request.ProjectId, cancellationToken);

            if (maybe.IsNone)
                return Result.Failure(new Error(ErrorCode.NotFound, "Данного проекта не существует."));

            Project project = maybe.Value;

            bool hasLinks = await assetRepository.HasProjectsLinksAsync(project.Id, cancellationToken);

            if (hasLinks)
                return Result.Failure(new Error(ErrorCode.Delete, "Нельзя удалить данный проект, т.к он привязан к другим асетам"));

            await unitOfWork.SaveChangesAsync(cancellationToken);
            projectRepository.Remove(project);

            return Result.Success();
        }
    }
}