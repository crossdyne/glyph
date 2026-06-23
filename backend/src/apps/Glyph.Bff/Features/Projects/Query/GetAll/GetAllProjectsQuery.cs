using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Bff.Features.Projects.Query.GetAll
{
    public sealed record GetAllProjectsQuery() : IRequest<List<ProjectResponse>>;
}