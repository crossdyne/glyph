using Crossdyne.Toolkit.Results;
using Glyph.Assets.Application.Interfaces;
using Glyph.Assets.Application.Interfaces.Repositories;
using Glyph.Assets.Domain.Models;
using Glyph.Assets.Domain.ValueObjects.Projects;
using MediatR;

namespace Glyph.Assets.Application.Features.Projects.Commands.Create
{
    public sealed class CreateProjectCommandHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, Result>
    {
        public async Task<Result> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project project = Project.Create(ProjectName.Create(request.Name), ProjectCode.Create(request.Code));

                await repository.AddAsync(project, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(new Error(ErrorCode.Create, $"Произошла критическая ошибка при создание проекта: {ex}"));
            }
        }
    }
}