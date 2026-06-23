using Glyph.Bff.Interfaces.Clients;
using MediatR;
using Shared.Contracts.Assets.Responses;

namespace Glyph.Bff.Features.Projects.Query.GetAll
{
    public sealed class GetAllProjectsQueryHandler(IProjectClient client) : IRequestHandler<GetAllProjectsQuery, List<ProjectResponse>>
    {
        public async Task<List<ProjectResponse>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
            => await client.GetAllAsync();
    }
}