using Crossdyne.Glyph.Application.Interfaces.Repositories;
using MediatR;
using Shared.Contracts.Responses;

namespace Crossdyne.Glyph.Application.Features.Projects.Queries.GetAll
{
    public sealed class GetAllProjectsQueryHandler(IProjectRepository repository) : IRequestHandler<GetAllProjectsQuery, List<ProjectResponse>>
    {
        public async Task<List<ProjectResponse>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
            => await repository.GetAllAsync();
    }
}