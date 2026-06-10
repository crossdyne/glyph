using Crossdyne.Glyph.Application.Interfaces;
using Crossdyne.Glyph.Application.Interfaces.Repositories;
using Crossdyne.Glyph.Domain.Models;
using Crossdyne.Glyph.Domain.ValueObjects.Projects;
using Crossdyne.Toolkit.Results;
using MediatR;

namespace Crossdyne.Glyph.Application.Features.Projects.Commands.Create
{
    public sealed class CreateProjectCommandHandler(
        IProjectRepository repository, 
        IUnitOfWork unitOfWork) : IRequestHandler<CreateProjectCommand, Result>
    {
        public async Task<Result> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Project project = Project.Create(ProjectName.Create(request.Name));

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