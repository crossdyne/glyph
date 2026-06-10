using MediatR;
using Shared.Contracts.Responses;

namespace Glyph.Application.Features.Projects.Queries.GetAll
{
    public sealed record GetAllProjectsQuery() : IRequest<List<ProjectResponse>>;
}